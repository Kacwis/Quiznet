import pt from "prop-types";
import { useContext, useEffect } from "react";
import AuthContext from "../../store/auth-context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faCirclePlay } from "@fortawesome/free-regular-svg-icons";
import { faEnvelope } from "@fortawesome/free-solid-svg-icons";
import useHttp from "../../hooks/use-http";
import { startGameWithFriend } from "../../api";

import style from "./Menu.module.css";
import GameContext from "../../store/game-context";
import { useNavigate } from "react-router-dom";

const RankingPlace = ({ player, openInbox, index }) => {
	const { loggedUser, token } = useContext(AuthContext);

	const { setGameById } = useContext(GameContext);

	const navigate = useNavigate();

	let placeNumberClassname;
	console.log(index);
	switch (index + 1) {
		case 1:
			placeNumberClassname = style["first-place"];
			break;
		case 2:
			placeNumberClassname = style["second-place"];
			break;
		case 3:
			placeNumberClassname = style["third-place"];
			break;
	}

	const usernameClassname = `${style.username} ${
		player.id === loggedUser.id ? style["logged-username"] : ""
	}`;

	const { status, error, data, sendRequest } = useHttp(startGameWithFriend);

	useEffect(() => {
		if (status === "completed" && !error) {
			setGameById(data.gameId);
			navigate(`/game/${data.gameId}`);
		}
	}, [status, error, setGameById, data, navigate]);

	const playClickHandler = (friend) => {
		const requestData = {
			token,
			createFriendGameDto: {
				startingPlayerId: loggedUser.id,
				friendId: friend.id,
			},
		};
		sendRequest(requestData);
	};

	return (
		<li key={player.username}>
			<div className={style.friend}>
				<div className={style["friend-inner"]}>
					<p className={placeNumberClassname}>{index + 1}</p>
					<p className={usernameClassname}>{player.username}</p>
					<p className={style.score}>{player.score}</p>
				</div>
				{player.id !== loggedUser.id && (
					<div className={style["friend-controls"]}>
						<FontAwesomeIcon
							icon={faCirclePlay}
							size="xl"
							onClick={() => playClickHandler(player)}
							style={{
								color: "var(--dark-pink-color)",
								marginLeft: "0.5rem",
								cursor: "pointer",
							}}
						/>
						<FontAwesomeIcon
							icon={faEnvelope}
							size="xl"
							style={{
								color: "var(--dark-pink-color)",
								marginLeft: "0.5rem",
								cursor: "pointer",
							}}
							onClick={() => openInbox(player.id)}
						/>
					</div>
				)}
			</div>
		</li>
	);
};

RankingPlace.propTypes = {
	player: pt.object.isRequired,
	openInbox: pt.func.isRequired,
	index: pt.number.isRequired,
};

export default RankingPlace;
