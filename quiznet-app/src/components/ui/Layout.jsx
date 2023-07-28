import PropTypes from "prop-types";

import MainNavigation from "../main-nav/MainNavigation";

const Layout = ({ children }) => {
	return (
		<div className="layout">
			<MainNavigation />
			<div className="layout-content">{children}</div>
		</div>
	);
};

Layout.propTypes = {
	children: PropTypes.node,
};

export default Layout;
