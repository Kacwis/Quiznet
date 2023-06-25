import MainNavigation from "../main-nav/MainNavigation";

const Layout = (props) => {
	return (
		<div>
			<MainNavigation />
			<div className="layout-content">{props.children}</div>
		</div>
	);
};

export default Layout;
