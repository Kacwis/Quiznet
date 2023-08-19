import { useContext } from "react";
import AuthContext from "../../store/auth-context";
import style from "./Settings.module.css";
import Modal from "../ui/Modal";
import AccountSettings from "./AccountSettings";

import pt from "prop-types";
import Friends from "./Friends";

const Settings = ({ closeSettings }) => {
	const { loggedUser } = useContext(AuthContext);

	return (
		<Modal className={style.main}>
			<div className={style.settings}>
				<button onClick={closeSettings}>X</button>
				<AccountSettings />
				<Friends />
				<div className={style.games}></div>
			</div>
		</Modal>
	);
};

Settings.propTypes = {
	closeSettings: pt.func,
};

export default Settings;
