import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import style from "./Settings.module.css";
import pt from "prop-types";
import { faPenToSquare } from "@fortawesome/free-regular-svg-icons";

const EditButton = ({ clickHandler }) => {
	return (
		<button onClick={clickHandler} className={style["change-btn"]}>
			<div className={style["change-btn-inner"]}>
				<p>Edit</p>
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
