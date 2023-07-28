const BASE_URL = "http://localhost:5246/api/";
const LOG_IN_URL = `${BASE_URL}auth/login`;
const GAME_URL = `${BASE_URL}game/`;
const CATEGORIES_API_URL = `${BASE_URL}category/`;
const QUESTIONS_API_URL = `${BASE_URL}questions/`;
const PLAYER_URL = `${BASE_URL}players/`;

const sendHttpGetRequest = async (url) => {
	const response = await fetch(url);
	const data = await response.json();
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	console.log(data);
	return data.result;
};

export const logIn = async (loginDTO) => {
	const response = await fetch(LOG_IN_URL, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(loginDTO),
	});

	const data = await response.json();
	if (!response.ok) {
		throw new Error("Something went horribly wrong!!");
	}
	console.log(data);
	return data;
};

export const startGameWithRandom = async (playerId) => {
	return await sendHttpGetRequest(`${GAME_URL}randomGame/${playerId}`);
};

export const getRandomCategories = async () => {
	return await sendHttpGetRequest(`${CATEGORIES_API_URL}random`);
};

export const getGameById = async (gameId) => {
	return await sendHttpGetRequest(`${GAME_URL}${gameId}`);
};

export const getActiveGames = async (playerId) => {
	return await sendHttpGetRequest(`${GAME_URL}player/${playerId}`);
};

export const getThreeRandomQuestionByCategory = async (category) => {
	return await sendHttpGetRequest(
		`${QUESTIONS_API_URL}random/${3}/by/${category}`
	);
};

export const getMenuData = async (requestData) => {
	const { playerId, token } = requestData;
	const response = await fetch(`${PLAYER_URL}menu/${playerId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	console.log(data);
	return data.result;
};

export const savePlayedRound = async (saveData) => {
	let method;
	let dataToStringify;
	let url;
	if (saveData.round === undefined) {
		method = "PUT";
		dataToStringify = { playerAnswers: saveData.playerAnswers };
		url = `${GAME_URL}round/${saveData.roundId}`;
	} else {
		method = "POST";
		dataToStringify = saveData.round;
		url = `${GAME_URL}${saveData.gameId}/round`;
	}
	console.log(saveData);
	const response = await fetch(url, {
		method: method,
		headers: {
			"Content-Type": "application/json",
		},
		body: JSON.stringify(dataToStringify),
	});
	const data = await response.json();
	if (!response.ok) {
		console.log(data);
		throw new Error(data.errorMessages);
	}
	console.log(data);
	return data.result;
};
