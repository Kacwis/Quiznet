import { useContext } from "react";
import style from "./Inbox.module.css";

import pt from "prop-types";
import LanguageContext from "../../store/language-context";
import { getAvatarPathByNumber } from "../../constants/Constants";

const ChatListItem = ({ chat, chatClickHandler }) => {
	const username = chat.chatReceiver.username;
	let text = chat.lastMessage.text;

	const { dictionary } = useContext(LanguageContext);

	if (chat.lastMessage.text === "PLAYER_INVITE") {
		text = `${username} ${dictionary.friendInviteText}`;
	}

	console.log(chat.chatReceiver);

	const avatarPath = getAvatarPathByNumber(chat.chatReceiver.avatarId);
	console.log(avatarPath);

	return (
		<li key={username}>
			<div
				className={style["chat-list-item"]}
				onClick={() => chatClickHandler(chat)}
			>
				<div>
					<img src={avatarPath} alt={"avatar"} className={style.avatar} />
				</div>
				<div className={style["chat-list-item-inner"]}>
					<h4>{username}</h4>
					<p>{text}</p>
				</div>
			</div>
		</li>
	);
};

ChatListItem.propTypes = {
	chat: pt.object,
	chatClickHandler: pt.func,
};

export default ChatListItem;
