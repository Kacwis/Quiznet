import { useContext, useEffect, useRef, useState } from "react";
import Modal from "../ui/Modal";
import useHttp from "../../hooks/use-http";
import { findPotentialFriends } from "../../api";
import AuthContext from "../../store/auth-context";
import AddFriendListItem from "./AddFriendListItem";

import style from "./Menu.module.css";

import pt from "prop-types";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMagnifyingGlass } from "@fortawesome/free-solid-svg-icons";
import LanguageContext from "../../store/language-context";

const AddFriendPanel = ({ closePanel }) => {
	const [foundFriendsList, setFoundFriendsList] = useState([]);

	const { token } = useContext(AuthContext);
	const { dictionary } = useContext(LanguageContext);

	const usernameRef = useRef();

	const { status, error, data, sendRequest } = useHttp(findPotentialFriends);

	useEffect(() => {
		if (status === "completed" && !error) {
			setFoundFriendsList(data);
		}
	}, [status, error, setFoundFriendsList, data]);

	const searchClickHandler = () => {
		const requestData = { token, username: usernameRef.current.value };
		sendRequest(requestData);
	};

	const foundFriendsListContent = foundFriendsList.map((friend) => {
		return (
			<AddFriendListItem
				friend={friend}
				closePanel={closePanel}
				key={friend.id}
			/>
		);
	});

	return (
		<Modal className={style["add-friend-panel"]}>
			<button onClick={() => closePanel()} className={style["exit-btn"]}>
				X
			</button>
			<div className={style["friend-search-cont"]}>
				<input
					type={"text"}
					placeholder={dictionary.playersUsername}
					ref={usernameRef}
				/>
				<FontAwesomeIcon
					icon={faMagnifyingGlass}
					onClick={searchClickHandler}
					size="2x"
					style={{ color: "var(--dark-pink-color)", marginLeft: "1rem" }}
				/>
			</div>
			{foundFriendsList.length !== 0 && (
				<div className={style["potential-friends-list"]}>
					<ul>{foundFriendsListContent}</ul>
				</div>
			)}
		</Modal>
	);
};

AddFriendPanel.propTypes = {
	closePanel: pt.func,
};

export default AddFriendPanel;
