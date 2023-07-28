import { useContext } from "react";
import AuthContext from "../../store/auth-context";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGear, faRightFromBracket } from "@fortawesome/free-solid-svg-icons";

import style from "./Menu.module.css";
import { useNavigate } from "react-router-dom";

const AccountInfo = () => {
	const authContext = useContext(AuthContext);

	const navigate = useNavigate();

	const exitClickHandler = () => {
		navigate("/");
		authContext.logOut();
	};

	return (
		<div className={style["account-info"]}>
			<div className={style["account-info-inner"]}>
				<p className={style.username}>{authContext.loggedUser.username}</p>
				<div className={style["auth-controls"]}>
					<FontAwesomeIcon
						icon={faGear}
						size="2x"
						style={{ margin: "0 0.5rem" }}
					/>
					<FontAwesomeIcon
						icon={faRightFromBracket}
						size="2x"
						style={{ margin: "0 0.5rem" }}
						onClick={exitClickHandler}
					/>
				</div>
			</div>
		</div>
	);
};

export default AccountInfo;
