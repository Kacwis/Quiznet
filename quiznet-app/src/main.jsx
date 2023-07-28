import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App.jsx";
import "./index.css";
import { BrowserRouter } from "react-router-dom";
import LanguageProvider from "./store/LanguageProvider.jsx";
import AuthProvider from "./store/AuthProvider.jsx";
import GameProvider from "./store/GameProvider.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
	<React.StrictMode>
		<AuthProvider>
			<LanguageProvider>
				<GameProvider>
					<BrowserRouter>
						<App />
					</BrowserRouter>
				</GameProvider>
			</LanguageProvider>
		</AuthProvider>
	</React.StrictMode>
);
