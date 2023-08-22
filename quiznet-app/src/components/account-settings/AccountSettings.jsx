import style from "./Settings.module.css";
import Avatar from "./Avatar";
import Username from "./Username";
import Password from "./Password";

const AccountSettings = () => {
	return (
		<div className={style.account}>
			<Avatar />
			<div className={style["account-options"]}>
				<Username />
				<Password />
			</div>
		</div>
	);
};

export default AccountSettings;
