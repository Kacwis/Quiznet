import pt from "prop-types";

import style from "./Inbox.module.css";
import { useContext, useEffect } from "react";
import LanguageContext from "../../store/language-context";
import useHttp from "../../hooks/use-http";
import AuthContext from "../../store/auth-context";
import { sendFriendshipDecision } from "../../api";

const REJECT_FLAG_VALUE = "REJECT";
const ACCEPT_FLAG_VALUE = "ACCEPT";

const InviteMessage = ({ msg }) => {
	const { dictionary } = useContext(LanguageContext);

	const { token } = useContext(AuthContext);

	const decisionClickHandler = (decision) => {
		const friendshipDecisionDTO = {
			senderId: msg.sender.id,
			receiverId: msg.receiver.id,
			decision,
		};
		const requestData = { token, friendshipDecisionDTO};
		sendFriendshipDecision(requestData);
	};

	return (
		<div className={style.msg}>
			<p>
				{msg.sender.username} {dictionary.friendInviteText}
			</p>
			<div>
				<button onClick={() => decisionClickHandler(ACCEPT_FLAG_VALUE)}>
					Accept
				</button>
				<button onClick={() => decisionClickHandler(REJECT_FLAG_VALUE)}>
					Reject
				</button>
			</div>
		</div>
	);
};

InviteMessage.propTypes = {
	msg: pt.object,
};

export default InviteMessage;
