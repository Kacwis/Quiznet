import style from "./Settings.module.css";
import Avatar from "./Avatar";
import Username from "./Username";
import Password from "./Password";
import { useState } from "react";

const AccountSettings = () => {
	return (
		<div className={style.account}>
			<Avatar />
			<div className={style["account-options"]}>
				<Username />
				<div className={style.email}>
					<label>janek@wp.pl</label>
				</div>
				<Password />
			</div>
		</div>
	);
};

export default AccountSettings;
