import { useContext, useEffect, useState } from "react";
import style from "./Settings.module.css";
import AuthContext from "../../store/auth-context";

import FriendListItem from "./FriendListItem";

const DEFAULT_RESPONSE_MSG_STATE = {
	message: "",
	className: style["friend-response-msg"],
};

const SHOW_TIME_MESSAGE = 5000;

const Friends = () => {
	const [currentResponseMsg, setCurrentResponseMsg] = useState(
		DEFAULT_RESPONSE_MSG_STATE
	);
	const { menuData } = useContext(AuthContext);

	const showResponseMessage = (msg, isPositive) => {
		const newClassname = `${currentResponseMsg.className} ${
			isPositive ? style.positive : style.negative
		}`;

		const newMsg = {
			message: msg,
			className: newClassname,
		};
		setCurrentResponseMsg(newMsg);
	};

	const hideMessage = () => {
		setCurrentResponseMsg(DEFAULT_RESPONSE_MSG_STATE);
	};

	useEffect(() => {
		let timeoutId;
		if (currentResponseMsg.message.length > 0) {
			timeoutId = setTimeout(() => {
				hideMessage();
			}, SHOW_TIME_MESSAGE);
		}
		return () => clearTimeout(timeoutId);
	}, [currentResponseMsg.message]);

	const friendsListContent = menuData.friends.map((friend) => {
		return (
			<FriendListItem
				friend={friend}
				key={friend.id}
				showResponseMsg={showResponseMessage}
			/>
		);
	});

	return (
		<div className={style.friends}>
			<h3>Friends</h3>
			<p className={currentResponseMsg.className}>
				{currentResponseMsg.message}
			</p>
			<ul>{friendsListContent}</ul>
		</div>
	);
};

export default Friends;
