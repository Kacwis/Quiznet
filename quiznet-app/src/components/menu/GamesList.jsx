import { useEffect, useState } from "react";
import GameListItem from "./GameListItem";
import style from "./Menu.module.css";

import { Link } from "react-router-dom";

import PropTypes from "prop-types";

const GamesList = ({ title, games }) => {
	const [currentGames, setCurrentGames] = useState(games);

	useEffect(() => {
		setCurrentGames(games);
	}, [games]);

	const gamesListContent = currentGames.map((game) => {
		return (
			<li key={game.id}>
				<Link to={`/game/${game.id}`}>
					<GameListItem game={game} />
				</Link>
			</li>
		);
	});

	return (
		<div className={style["your-games"]}>
			<h2>{title}</h2>
			<div className={style["games-list"]}>
				<ul>{gamesListContent}</ul>
			</div>
		</div>
	);
};

GamesList.propTypes = {
	title: PropTypes.string,
	games: PropTypes.array,
};

export default GamesList;
