import Modal from "../ui/Modal";
import style from "./Inbox.module.css";
import pt from "prop-types";
import useHttp from "../../hooks/use-http";
import LoadingSpinner from "../ui/LoadingSpinner";
import { getChatsList } from "../../api";
import { useContext, useEffect, useState } from "react";
import AuthContext from "../../store/auth-context";
import ChatsList from "./ChatsList";
import Chat from "./Chat";
import { faTableCellsLarge } from "@fortawesome/free-solid-svg-icons";

const CHAT_LIST_REQUEST_REFRESH_TIME_MILISECONDS = 1000 * 15;

const Inbox = ({ closeInbox, chatPlayerId }) => {
	const [chatsList, setChatsList] = useState([]);
	const [currentChat, setCurrentChat] = useState(null);

	const { token, loggedUser, getMenuData } = useContext(AuthContext);

	const { status, error, data, sendRequest } = useHttp(getChatsList);

	const exitClickHandler = () => {
		getMenuData();
		closeInbox();
	};

	useEffect(() => {
		const requestData = { token, playerId: loggedUser.id };
		sendRequest(requestData);
		setCurrentChat(null);
		const intervalId = setInterval(() => {
			sendRequest(requestData);
		}, CHAT_LIST_REQUEST_REFRESH_TIME_MILISECONDS);
		return () => {
			clearInterval(intervalId);
		};
	}, []);

	useEffect(() => {
		if (status === "completed" && !error) {
			setChatsList(data);
			const chat = data.filter((chat) => chat.chatReceiver.id === chatPlayerId);
			console.log(chat);
			console.log(chatPlayerId);
			if (chatPlayerId) {
				if (chat.length > 0) {
					setCurrentChat(chat[0]);
				} else {
					setCurrentChat({
						chatReceiver: {
							id: chatPlayerId,
						},
						messages: [],
					});
				}
			}
		}
	}, [status, error, data, setChatsList, chatPlayerId]);

	const setChat = (chat) => {
		if (currentChat === chat) {
			setCurrentChat(null);
			return;
		}
		setCurrentChat(chat);
	};


	return (
		<Modal className={style.main}>
			<div className={style["inbox-controls"]}>
				<button onClick={exitClickHandler}>X</button>
			</div>
			<div className={style["inbox-inner"]}>
				<ChatsList chatsList={chatsList} setCurrentChat={setChat} />
				{currentChat && <Chat receiverId={currentChat.chatReceiver.id} />}
			</div>
		</Modal>
	);
};

Inbox.propTypes = {
	closeInbox: pt.func,
	chatPlayerId: pt.number,
};

export default Inbox;
