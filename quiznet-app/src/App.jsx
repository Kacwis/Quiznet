import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Layout from "./components/ui/Layout";
import StartPanel from "./components/main-page/StartPanel";

function App() {
	return (
		<Layout>
			<Routes>
				<Route path="/start" element={<StartPanel />} />
			</Routes>
		</Layout>
	);
}

export default App;
