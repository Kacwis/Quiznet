import { useContext, useEffect } from "react";
import AuthContext from "../../store/auth-context";
import useHttp from "../../hooks/use-http";
import LoadingSpinner from "../ui/LoadingSpinner";
import { registerNewGuestAccount } from "../../api";
import { useNavigate } from "react-router-dom";

const GuestPanel = () => {
	const { logIn, getMenuData } = useContext(AuthContext);

	const navigate = useNavigate();

	const { status, error, data, sendRequest } = useHttp(
		registerNewGuestAccount,
		true
	);

	useEffect(() => {
		sendRequest();
	}, []);

	useEffect(() => {
		if (status === "completed" && !error) {
			logIn(data);
			setTimeout(() => {
				getMenuData();
				navigate("/menu");
			}, [10000]);
		}
	}, [status, error, data, logIn, navigate, getMenuData]);

	if (status === "pending") {
		return <LoadingSpinner />;
	}

	return (
		<div>
			<p>
				Your guest information will be saved for one day in the browser storage
			</p>
			<p>
				In order to completely enjoy all game features. We would recomend to
				create an account
			</p>
		</div>
	);
};

export default GuestPanel;
