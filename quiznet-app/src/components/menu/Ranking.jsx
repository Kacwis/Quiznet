import { useContext, useState } from "react";
import style from "./Menu.module.css";

import pt from "prop-types";

import AuthContext from "../../store/auth-context";
import RankingPlace from "./RankingPlace";

const Ranking = ({ openInbox }) => {
	const { menuData } = useContext(AuthContext);

	const friendsListContent = menuData.friends.map((friend, index) => {
		return (
			<RankingPlace
				player={friend}
				openInbox={openInbox}
				index={index}
				key={index}
			/>
		);
	});

	return (
		<div className={style["friends-ranking"]}>
			<h2>Ranking</h2>
			<ol>{friendsListContent}</ol>
		</div>
	);
};
1;

Ranking.propTypes = {
	openInbox: pt.func,
};

export default Ranking;
