import PropTypes from "prop-types";

const CentralPanel = ({ className, children }) => {
	const classNames = `central-panel ${className}`;
	return <div className={classNames}>{children}</div>;
};

CentralPanel.propTypes = {
	className: PropTypes.string,
	children: PropTypes.node,
};

export default CentralPanel;
