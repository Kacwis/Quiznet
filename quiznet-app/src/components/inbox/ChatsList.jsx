import { useEffect, useState } from "react";
import style from "./Inbox.module.css";

import pt from "prop-types";
import ChatListItem from "./ChatListItem";

const ChatsList = ({ chatsList, setCurrentChat }) => {
	const [currentChatsList, setCurrentChatsList] = useState(chatsList);

	useEffect(() => {
		setCurrentChatsList(chatsList);
	}, [chatsList]);

	const chatClickHandler = (chat) => {
		setCurrentChat(chat);
	};

	const chatsListContent = currentChatsList.map((chat) => (
		<ChatListItem
			key={chat.chatReceiver.username}
			chat={chat}
			chatClickHandler={chatClickHandler}
		/>
	));

	return (
		<div className={style["chats-list"]}>
			<ul>{chatsListContent}</ul>
		</div>
	);
};

ChatsList.propTypes = {
	chatsList: pt.array,
	setCurrentChat: pt.func,
};

export default ChatsList;
