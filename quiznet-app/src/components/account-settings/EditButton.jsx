import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import style from "./Settings.module.css";
import pt from "prop-types";
import { faPenToSquare } from "@fortawesome/free-regular-svg-icons";
import { useContext } from "react";
import LanguageContext from "../../store/language-context";

const EditButton = ({ clickHandler }) => {
	const { dictionary } = useContext(LanguageContext);

	return (
		<button onClick={clickHandler} className={style["change-btn"]}>
			<div className={style["change-btn-inner"]}>
				<p>{dictionary.edit}</p>
				<FontAwesomeIcon
					icon={faPenToSquare}
					style={{ marginLeft: "0.5rem", height: "1.2rem" }}
				/>
			</div>
		</button>
	);
};

EditButton.propTypes = {
	clickHandler: pt.func,
};

export default EditButton;
