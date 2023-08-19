import ReactDOM from "react-dom";

import PropTypes from "prop-types";

const Backdrop = () => {
	return <div className={"backdrop"}></div>;
};

const ModalOverlay = ({ children, className }) => {
	const modalClassname = `modal ${className}`;

	return (
		<div className={modalClassname}>
			<div className={"content"}>{children}</div>
		</div>
	);
};

const portalElement = document.getElementById("overlays");

const Modal = ({ children, className }) => {
	return (
		<>
			{ReactDOM.createPortal(<Backdrop />, portalElement)}
			{ReactDOM.createPortal(
				<ModalOverlay className={className}>{children}</ModalOverlay>,
				portalElement
			)}
		</>
	);
};

ModalOverlay.propTypes = {
	children: PropTypes.node,
	className: PropTypes.string,
};

Modal.propTypes = {
	children: PropTypes.node,
	className: PropTypes.string,
};

export default Modal;
