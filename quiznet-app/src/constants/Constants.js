export const getPlayersScoreInGame = (game, player) => {
	let playerScore = 0;
	let opponentScore = 0;

	game.rounds.forEach((round) =>
		round.playerAnswers.forEach((answer) => {
			if (answer.player.id === player.id && answer.isCorrect) {
				playerScore += 1;
			} else if (answer.player.id !== player.id && answer.isCorrect) {
				opponentScore += 1;
			}
		})
	);
	return { playerScore, opponentScore };
};

export const getCurrentTurn = (activeRound, isStarting) => {
	const isAnswersEmpty = activeRound && activeRound.playerAnswers.length === 0;

	const isPlayerTurn =
		activeRound && activeRound.roundNumber % 2 === 1
			? isStarting ^ !isAnswersEmpty
			: !(isStarting ^ !isAnswersEmpty);
	return isPlayerTurn;
};

const NUMBER_OF_AVATARS = 5;

export const getAvatarPathByNumber = (number) => {
	if (number > NUMBER_OF_AVATARS && number < 1) {
		throw new Error("There are no avatars with that numbers");
	}
	return `/src/assets/Avatar${number}.png`;
};

export const AVATARS = [
	{
		id: 1,
		path: "/src/assets/Avatar1.png",
		alt: "Blue alien creature with horns and yellow eyes",
		altPl: "Niebieskie pozaziemskie stoworzenie z rogami i żółtymi oczami",
	},
	{
		id: 2,
		path: "/src/assets/Avatar2.png",
		alt: "Green alien creature in astronaut suit",
		altPl: "Zielona pozaziemska istota w stroju astronauty",
	},
	{
		id: 3,
		path: "/src/assets/Avatar3.png",
		alt: "Some creature in helmet with black visor and light bluish armour",
		altPl: "Jakaś istota w hełmie z czarną przyłbicą i niebieskawym strojem ",
	},
	{
		id: 4,
		path: "/src/assets/Avatar4.png",
		alt: "Cartoon Astronaut with an american flag on the shoulder",
		altPl: "Kreskówkowy astronauta z amerykańska flagą na ramieniu",
	},
	{
		id: 5,
		path: "/src/assets/Avatar5.png",
		alt: "Blue eyed girl with helmet and astronout suit",
		altPl: "Niebisekooka dziewczyna z hełmem i strojemu astronauty",
	},
];

export const SETTINGS_FRIENDS_ICONS_STYLE = {
	color: "var(--dark-pink-color)",
	cursor: "pointer",
	height: "1.75rem",
	margin: "0 0.5rem",
	transition: "transform 0.2s ease-in-out",
};
