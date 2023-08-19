import pt from "prop-types";
import { useContext, useEffect, useRef, useState } from "react";
import useHttp from "../../hooks/use-http";
import { getChat, sendMessage, updateIsReadMessages } from "../../api";
import AuthContext from "../../store/auth-context";

import style from "./Inbox.module.css";
import InviteMessage from "./InviteMessage";
import LanguageContext from "../../store/language-context";

const CHAT_REQUEST_REFRESH_TIME_MILISECONDS = 1000 * 10;

const DAY_IN_MILISECONDS = 1000 * 60 * 60 * 24;

const Chat = ({ receiverId }) => {
	const [chatData, setChatData] = useState({
		chatReceiver: null,
		messages: [],
	});

	const { token, loggedUser } = useContext(AuthContext);

	const msgInputRef = useRef();

	const chatWindowRef = useRef();

	const { dictionary } = useContext(LanguageContext);

	const { status, error, data, sendRequest } = useHttp(getChat, true);

	useEffect(() => {
		if (chatData.messages.filter((msg) => !msg.isRead).length > 0) {
			const requestData = { token, receiverId };
			updateIsReadMessages(requestData);
		}
	}, [chatData]);

	useEffect(() => {
		const requestData = { token, receiverId };
		sendRequest(requestData);
		const intervalId = setInterval(() => {
			sendRequest(requestData);
		}, CHAT_REQUEST_REFRESH_TIME_MILISECONDS);

		return () => {
			clearInterval(intervalId);
		};
	}, [token, receiverId, sendRequest]);

	useEffect(() => {
		if (status === "completed" && !error) {
			console.log(data);
			data.messages.reverse();
			if (chatWindowRef.current && data.messages !== chatData.messages) {
				chatWindowRef.current.scrollTop = chatWindowRef.current.scrollHeight;
			}
			setChatData(data);
		}
	}, [status, error, setChatData, data, chatWindowRef, chatData]);

	const getDate = (date) => {
		const parsedDate = new Date(Date.parse(date));
		if (Date.now() - parsedDate > DAY_IN_MILISECONDS) {
			return `${parsedDate.getDate()} ${
				dictionary.monthsNamesShort[parsedDate.getMonth() - 1]
			} ${parsedDate.getFullYear()}`;
		}
		return `${parsedDate.getHours()}:${parsedDate.getMinutes()}`;
	};

	const chatListContent = chatData.messages.map((msg, index) => {
		if (msg.text === "PLAYER_INVITE") {
			return <InviteMessage msg={msg} key={index} />;
		}
		const msgClassname = `${style.message} ${
			msg.sender.id === loggedUser.id
				? style["user-message"]
				: style["receiver-message"]
		}`;

		const date = getDate(msg.sendAt);
		return (
			<div key={index} className={msgClassname}>
				<p className={style["msg-date"]}>{date}</p>
				<p className={style["msg-text"]}>{msg.text}</p>
			</div>
		);
	});

	const sendClickHandler = () => {
		const msg = msgInputRef.current.value;
		console.log(msg);
		if (msg.length === 0 || msg.trim(" ").length === 0) {
			return;
		}
		msgInputRef.current.value = "";
		const requestData = {
			token,
			messageDTO: {
				senderId: loggedUser.id,
				receiverId,
				text: msg,
			},
		};
		setTimeout(() => {
			sendRequest({ token, receiverId });
		}, 200);
		const data = sendMessage(requestData);
		console.log(data);
	};

	return (
		<div className={style.chat}>
			<div className={style["chat-window"]} ref={chatWindowRef}>
				{chatListContent}
			</div>
			<div className={style["send-msg"]}>
				<input
					type="text"
					ref={msgInputRef}
					className={style["msg-input"]}
					placeholder={
						chatData.messages.length === 0 ? "Send your first message!" : ""
					}
				/>
				<button onClick={sendClickHandler} className={style.send}>
					Send
				</button>
			</div>
		</div>
	);
};

Chat.propTypes = {
	receiverId: pt.number,
};

export default Chat;
