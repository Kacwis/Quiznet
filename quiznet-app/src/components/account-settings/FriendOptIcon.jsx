import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import pt from "prop-types";
import { useContext, useEffect, useRef } from "react";
import useHttp from "../../hooks/use-http";
import AuthContext from "../../store/auth-context";
import { SETTINGS_FRIENDS_ICONS_STYLE } from "../../constants/Constants";

const FriendOptIcon = ({
	apiCall,
	icon,
	friendId,
	showResponseMsg,
	option,
}) => {
	const iconRef = useRef();

	const { status, error, data, sendRequest } = useHttp(apiCall);

	const { loggedUser, token, getMenuData } = useContext(AuthContext);

	const mouseEnterHandler = () => {
		iconRef.current.style.transform = "scale(1.15)";
	};

	const mouseLeaveHandler = () => {
		iconRef.current.style.transform = "scale(1)";
	};

	useEffect(() => {
		if (status === "completed" && !error) {
			if (data.isSuccess) {				
				showResponseMsg(`Friend was ${option}ed successfully`, true);
			} else {
				showResponseMsg(`Something went wrong!`, false);
			}
		}
	}, [status, error, data, getMenuData, showResponseMsg, option]);

	const clickHandler = () => {
		const requestData = {
			token,
			body: {
				senderId: loggedUser.id,
				friendId: friendId,
			},
		};
		sendRequest(requestData);
	};

	return (
		<FontAwesomeIcon
			icon={icon}
			ref={iconRef}
			style={SETTINGS_FRIENDS_ICONS_STYLE}
			onMouseEnter={mouseEnterHandler}
			onMouseLeave={mouseLeaveHandler}
			onClick={clickHandler}
		/>
	);
};

FriendOptIcon.propTypes = {
	apiCall: pt.func.isRequired,
	icon: pt.object.isRequired,
	friendId: pt.number.isRequired,
	showResponseMsg: pt.func.isRequired,
	option: pt.string.isRequired,
};

export default FriendOptIcon;
