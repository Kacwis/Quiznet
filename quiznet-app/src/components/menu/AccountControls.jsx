import { useContext, useState } from "react";
import AuthContext from "../../store/auth-context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
	faEnvelope,
	faGear,
	faRightFromBracket,
	faUserPlus,
} from "@fortawesome/free-solid-svg-icons";
import AddFriendPanel from "./AddFriendPanel";

import style from "./Menu.module.css";
import { useNavigate } from "react-router-dom";

import pt from "prop-types";
import Settings from "../account-settings/Settings";
import { getAvatarPathByNumber } from "../../constants/Constants";

const AccountControls = ({ openInbox }) => {
	const [isAddFriendVisible, setIsAddFriendVisible] = useState(false);
	const [areSettingsVisible, setAreSettingsVisible] = useState(false);
	const authContext = useContext(AuthContext);

	const navigate = useNavigate();

	const { menuData, getMenuData, loggedUser } = useContext(AuthContext);

	const exitClickHandler = () => {
		navigate("/");
		authContext.logOut();
	};

	const closeAddFriendPanel = () => {
		getMenuData();
		setIsAddFriendVisible(false);
	};

	const settingsClickHandler = () => {
		setAreSettingsVisible(true);
	};

	const inboxIconContainerClassname = menuData.isNewMessages
		? style["icon-container"]
		: "";

	const iconStyle = { margin: "1rem 0.75rem", cursor: "pointer" };

	return (
		<>
			<div className={style["account-info"]}>
				<div className={style["account-info-inner"]}>
					<div className={style["avatar-username"]}>
						<label>{loggedUser.username}</label>
						<img src={getAvatarPathByNumber(loggedUser.avatarId)} />
					</div>
					<div className={style["auth-controls"]}>
						<FontAwesomeIcon
							icon={faUserPlus}
							size={"2x"}
							style={iconStyle}
							onClick={() => setIsAddFriendVisible(true)}
						/>
						<div className={inboxIconContainerClassname}>
							<FontAwesomeIcon
								icon={faEnvelope}
								size={"2x"}
								style={iconStyle}
								onClick={openInbox}
							/>
						</div>
						<FontAwesomeIcon
							icon={faGear}
							size={"2x"}
							style={iconStyle}
							onClick={settingsClickHandler}
						/>

						<FontAwesomeIcon
							icon={faRightFromBracket}
							size={"2x"}
							style={iconStyle}
							onClick={exitClickHandler}
						/>
					</div>
				</div>
			</div>
			{isAddFriendVisible && (
				<AddFriendPanel closePanel={closeAddFriendPanel} />
			)}
			{areSettingsVisible && (
				<Settings closeSettings={() => setAreSettingsVisible(false)} />
			)}
		</>
	);
};

AccountControls.propTypes = {
	openInbox: pt.func,
};

export default AccountControls;
