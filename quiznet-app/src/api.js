const BASE_URL = "http://localhost:5246/api/";
const LOG_IN_URL = `${BASE_URL}auth/login`;
const SIGN_UP_URL = `${BASE_URL}auth/register`;
const GAME_URL = `${BASE_URL}game/`;
const CATEGORIES_API_URL = `${BASE_URL}category/`;
const QUESTIONS_API_URL = `${BASE_URL}questions/`;
const PLAYER_URL = `${BASE_URL}players/`;
const MESSAGES_URL = `${BASE_URL}messages/`;

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

export const startGameWithRandom = async (requestData) => {
	const { token, playerId } = requestData;
	const response = await fetch(`${GAME_URL}randomGame/${playerId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (data.statusCode === 404) {
		return null;
	}
	return data.result;
};

export const getRandomCategories = async () => {
	return await sendHttpGetRequest(`${CATEGORIES_API_URL}random`);
};

export const getGameById = async (requestData) => {
	const { token, gameId } = requestData;
	const response = await fetch(`${GAME_URL}${gameId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const setPlayerActivity = async (playerId) => {
	const response = await fetch(`${PLAYER_URL}${playerId}/status`);
	const data = await response.json();
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
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
	const { id, token } = requestData;
	const response = await fetch(`${PLAYER_URL}menu/${id}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	console.log(data, "menuData");
	return data.result;
};

export const savePlayedRound = async (saveData) => {
	const { round, gameId, roundId, playerAnswers, token } = saveData;
	let method;
	let dataToStringify;
	let url;
	if (round === undefined) {
		method = "PUT";
		dataToStringify = { playerAnswers: playerAnswers };
		url = `${GAME_URL}round/${roundId}`;
	} else {
		method = "POST";
		dataToStringify = round;
		url = `${GAME_URL}${gameId}/round`;
	}
	console.log(saveData);
	const response = await fetch(url, {
		method: method,
		headers: {
			"Content-Type": "application/json",
			Authorization: `Bearer ${token}`,
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

export const registerNewGuestAccount = async () => {
	return sendHttpGetRequest(`${PLAYER_URL}guest`);
};

export const findPotentialFriends = async (requestData) => {
	const { token, username } = requestData;
	const response = await fetch(`${PLAYER_URL}findFriends/${username}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const addFriend = async (requestData) => {
	const { token, friendshipDTO } = requestData;
	const response = await fetch(`${PLAYER_URL}friend`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-type": "application/json",
		},
		body: JSON.stringify(friendshipDTO),
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const getChatsList = async (requestData) => {
	const { token, playerId } = requestData;
	const response = await fetch(`${MESSAGES_URL}chats/${playerId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result.chatList;
};

export const getChat = async (requestData) => {
	const { token, receiverId } = requestData;
	const response = await fetch(`${MESSAGES_URL}chat/${receiverId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const sendMessage = async (requestData) => {
	const { token, messageDTO } = requestData;
	const response = await fetch(`${MESSAGES_URL}`, {
		method: "POST",
		headers: {
			"Content-type": "application/json",
			Authorization: `Bearer ${token}`,
		},
		body: JSON.stringify(messageDTO),
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const sendFriendshipDecision = async (requestData) => {
	const { token, friendshipDecisionDTO } = requestData;
	const response = await fetch(`${PLAYER_URL}friend/decision`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-type": "application/json",
		},
		body: JSON.stringify(friendshipDecisionDTO),
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
};

export const startGameWithFriend = async (requestData) => {
	const { token, createFriendGameDto } = requestData;
	const response = await fetch(`${GAME_URL}friend`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-type": "application/json",
		},
		body: JSON.stringify(createFriendGameDto),
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data.result;
};

export const updateIsReadMessages = async (requestData) => {
	const { token, receiverId } = requestData;
	const response = await fetch(`${MESSAGES_URL}${receiverId}`, {
		method: "PUT",
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
};

export const signUp = async (requestData) => {
	const postData = { ...requestData, role: "ROLE_USER" };
	const response = await fetch(`${SIGN_UP_URL}`, {
		method: "POST",
		headers: {
			"Content-type": "application/json",
		},
		body: JSON.stringify(postData),
	});
	const data = await response.json();
	console.log(data);
	if (!response.ok) {
		throw new Error(data.errorMessages);
	}
	return data;
};

export const updateAvatarId = async (requestData) => {
	const { token, body } = requestData;
	const { playerId, avatarId } = body;
	const response = await fetch(`${PLAYER_URL}${playerId}/avatar/${avatarId}`, {
		method: "PUT",
		headers: {
			Authorization: `Bearer ${token}`,
		},
	});
	const data = await response.json();
	console.log(data);
	if (!data.isSuccess) {
		throw new Error(data.errorMessages);
	}
};

export const updateUsername = async (requestData) => {
	const { token, body } = requestData;
	const { playerId, username } = body;
	const response = await fetch(
		`${PLAYER_URL}${playerId}/username/${username}`,
		{
			method: "PUT",
			headers: {
				Authorization: `Bearer ${token}`,
			},
		}
	);
	const data = await response.json();
	return data;
};

export const updatePassword = async (requestData) => {
	const { token, body } = requestData;
	const { playerId, password } = body;
	const response = await fetch(`${PLAYER_URL}${playerId}/password`, {
		method: "PUT",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-type": "application/json",
		},
		body: JSON.stringify({
			password,
		}),
	});
	const data = await response.json();
	return data;
};

export const blockFriend = async (requestData) => {
	const { token, body } = requestData;
	const response = await fetch(`${PLAYER_URL}friend/block`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
	});
	const data = await response.json();
	return data;
};

export const removeFriend = async (requestData) => {
	const { token, body } = requestData;
	const response = await fetch(`${PLAYER_URL}unfriend`, {
		method: "POST",
		headers: {
			Authorization: `Bearer ${token}`,
			"Content-type": "application/json",
		},
		body: JSON.stringify(body),
	});
	const data = await response.json();
	return data;
};
