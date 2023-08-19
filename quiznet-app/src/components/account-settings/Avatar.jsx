import { useContext, useEffect, useState } from "react";
import style from "./Settings.module.css";
import AuthContext from "../../store/auth-context";
import LanguageContext from "../../store/language-context";
import { getAvatarPathByNumber } from "../../constants/Constants";
import { AVATARS } from "../../constants/Constants";

import { updateAvatarId } from "../../api";
import EditButton from "./EditButton";

const Avatar = () => {
	const [isEditAvatarVisible, setIsEditAvatarVisible] = useState(false);
	const [selectedAvatar, setSelectedAvatar] = useState(null);

	const { loggedUser, token, getMenuData } = useContext(AuthContext);

	const { activeLang } = useContext(LanguageContext);

	const avatarPath = getAvatarPathByNumber(loggedUser.avatarId);

	const selectAvatar = (avatar) => {
		if (avatar === selectedAvatar) {
			setSelectedAvatar(null);
			return;
		}
		setSelectedAvatar(avatar);
	};

	useEffect(() => {
		if (!isEditAvatarVisible) {
			setSelectedAvatar(null);
		}
	}, [isEditAvatarVisible, setSelectedAvatar]);

	const applyClickHandler = () => {
		const requestData = {
			token,
			body: {
				playerId: loggedUser.id,
				avatarId: selectedAvatar.id,
			},
		};
		updateAvatarId(requestData);
		setTimeout(() => getMenuData(), 200);
	};

	const avatarsListContent = AVATARS.map((avatar) => {
		let avatarClassname = style["avatar-edit-img"];
		if (avatar.path === avatarPath) {
			return;
		}
		if (avatar === selectedAvatar) {
			avatarClassname = `${avatarClassname} ${style["selected-avatar"]}`;
		}
		return (
			<li key={avatar.id}>
				<img
					src={avatar.path}
					alt={activeLang === "pl" ? avatar.altPl : avatar.alt}
					onClick={() => selectAvatar(avatar)}
					className={avatarClassname}
				/>
			</li>
		);
	});

	return (
		<div className={style["avatar-outer"]}>
			<div className={style.avatar}>
				<img src={avatarPath} alt={`${loggedUser.username}'s avatar`} />
				<EditButton
					clickHandler={() => {
						setIsEditAvatarVisible(!isEditAvatarVisible);
					}}
				/>
			</div>
			{isEditAvatarVisible && (
				<div className={style["avatar-edit"]}>
					<h4>Choose new Avatar</h4>
					<ul>{avatarsListContent}</ul>
					{selectedAvatar !== null && (
						<button onClick={applyClickHandler} className={style["apply-btn"]}>
							Apply
						</button>
					)}
				</div>
			)}
		</div>
	);
};

export default Avatar;
