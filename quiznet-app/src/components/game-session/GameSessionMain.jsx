import { useContext, useEffect, useState } from "react";
import CentralPanel from "../ui/CentralPanel";
import style from "./GameSession.module.css";
import GameSessionButtons from "./GameSessionButtons";
import { useParams } from "react-router-dom";
import LoadingSpinner from "../ui/LoadingSpinner";
import PlayersInfo from "./PlayersInfo";
import GameSessionRound from "./GameSessionRound";
import AuthContext from "../../store/auth-context";
import GameContext from "../../store/game-context";
import GameMain from "../game/GameMain";
import PlayerAnswerDisplay from "./PlayerAnswerDisplay";

const GameSessionMain = () => {
	const [isAnswerDisplayVisible, setIsAnswerDisplayVisible] = useState(false);
	const [answerToDisplay, setAnswerToDisplay] = useState(null);

	const params = useParams();

	const { startRound, isRoundInPlay, activeGame, setGameById } =
		useContext(GameContext);

	const authContext = useContext(AuthContext);

	useEffect(() => {
		setGameById(params.gameId);
	}, [params.gameId]);

	if (activeGame === null) {
		return (
			<CentralPanel>
				<LoadingSpinner />
			</CentralPanel>
		);
	}

	const playRoundClickHandler = () => {
		startRound();
	};

	const isStarting =
		activeGame !== null &&
		activeGame.startingPlayerId === authContext.loggedUser.id;

	const isRoundsEmpty = activeGame !== null && activeGame.rounds.length === 0;

	return (
		<CentralPanel className={style.game}>
			{!isRoundInPlay && (
				<>
					<div
						style={{
							display: "flex",
							width: "100%",
							justifyContent: "flex-end",
						}}
					>
						<GameSessionButtons />
					</div>
					{activeGame !== null && (
						<>
							<PlayersInfo players={activeGame.players} />
							{isRoundsEmpty && (
								<div className={style["round-controls"]}>
									{isStarting ? (
										<button onClick={playRoundClickHandler}>Play</button>
									) : (
										<p>Wait for your turn</p>
									)}
								</div>
							)}
							<div className={style["game-rounds"]}>
								{activeGame.rounds.map((round, index) => {
									return <GameSessionRound gameRound={round} key={index} />;
								})}
							</div>
						</>
					)}
				</>
			)}
			{isRoundInPlay && <GameMain />}
			{isAnswerDisplayVisible && <PlayerAnswerDisplay />}
		</CentralPanel>
	);
};

export default GameSessionMain;
