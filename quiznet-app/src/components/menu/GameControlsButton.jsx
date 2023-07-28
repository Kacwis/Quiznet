import style from "./Menu.module.css";
import PropTypes from "prop-types";

const GameControlsButton = ({ clickHandler, text }) => {
	return (
		<button onClick={clickHandler} className={style["game-controls-btn"]}>
			{text}
		</button>
	);
};

GameControlsButton.propTypes = {
	clickHandler: PropTypes.func,
	text: PropTypes.string,
};

export default GameControlsButton;
