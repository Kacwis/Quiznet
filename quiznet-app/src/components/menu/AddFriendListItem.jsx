import { useContext, useEffect } from "react";
import useHttp from "../../hooks/use-http";
import style from "./Menu.module.css";

import PropTypes from "prop-types";
import AuthContext from "../../store/auth-context";
import { addFriend } from "../../api";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus } from "@fortawesome/free-solid-svg-icons";

const AddFriendListItem = ({ friend, closePanel }) => {
	const { status, error, data, sendRequest } = useHttp(addFriend);

	const { token, loggedUser, getMenuData } = useContext(AuthContext);

	useEffect(() => {
		if (status === "completed" && !error) {
			getMenuData();
			closePanel();
		}
	}, [status, error, data, closePanel, getMenuData]);

	const addFriendClickHandler = () => {
		const requestData = {
			token,
			friendshipDTO: {
				senderId: loggedUser.id,
				receiverId: friend.id,
			},
		};
		sendRequest(requestData);
	};

	return (
		<li key={friend.username} className={style["add-friend-list-item"]}>
			<label>{friend.username}</label>
			<FontAwesomeIcon
				icon={faPlus}
				onClick={addFriendClickHandler}
				className={style["add-friend-btn"]}
				size="xl"
				style={{
					border: "2px solid var(--dark-pink-color)",
					borderRadius: "50%",
					padding: "0.3rem 0.5rem 0.3rem 0.5rem",
					color: "var(--dark-pink-color)",
					cursor: "pointer",
				}}
			/>
		</li>
	);
};

AddFriendListItem.propTypes = {
	friend: PropTypes.object,
	closePanel: PropTypes.func,
};

export default AddFriendListItem;
