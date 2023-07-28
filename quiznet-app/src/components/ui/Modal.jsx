import ReactDOM from "react-dom";

import PropTypes from "prop-types";

const Backdrop = () => {
	return <div className={"backdrop"}></div>;
};

const ModalOverlay = ({ children }) => {
	return (
		<div className={"modal"}>
			<div className={"content"}>{children}</div>
		</div>
	);
};

const portalElement = document.getElementById("overlays");

const Modal = ({ children }) => {
	return (
		<>
			{ReactDOM.createPortal(<Backdrop />, portalElement)}
			{ReactDOM.createPortal(
				<ModalOverlay>{children}</ModalOverlay>,
				portalElement
			)}
		</>
	);
};

ModalOverlay.propTypes = {
	children: PropTypes.node,
};

Modal.propTypes = {
	children: PropTypes.node,
};

export default Modal;
