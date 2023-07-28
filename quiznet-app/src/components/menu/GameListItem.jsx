import { useContext } from "react";
import style from "./Menu.module.css";
import AuthContext from "../../store/auth-context";

import PropTypes from "prop-types";

const GameListItem = ({ game }) => {
	const authContext = useContext(AuthContext);

	return (
		<div className={style["game-list-item"]}>
			<div className={style.opponent}>
				<img src="https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/avatars/49/4938f216e85152f6f85ee01c422e1ecdc9730dd1_full.jpg" />
				<p>
					{
						game.players.find((p) => p.id !== authContext.loggedUser.id).user
							.username
					}
				</p>
			</div>
			<p>{game.result}</p>
		</div>
	);
};

GameListItem.propTypes = {
	game: PropTypes.object,
};

export default GameListItem;
