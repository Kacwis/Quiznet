# Quiznet

Welcome to the Quiznet project! This project is a web-based quiz game that allows users to play turn-based multiplayer quizzes with friends, contact them, and compete for the top rankings. The backend of the game is developed using .NET Web API and SQL database, while the frontend is built using React.js.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Backend Installation](#backend-installation)
- [Frontend Installation](#frontend-installation)
- [Usage](#usage)
- [Design Decisions](#design-decisions)
- [Future Plans and Improvements](#future-plans)
- [Future Updates](#future-updates)


## Introduction

The Quiznet provides an engaging platform for users to challenge their friends in a quiz battle. Players can select from various categories and answer questions in a turn-based manner. The game also includes an inbox system for messaging between friends and a ranking system to showcase the top players.

## Features

- **Multiplayer Quizzes:** Play turn-based multiplayer quizzes with friends.
- **Various Categories:** Choose from a wide range of quiz categories to test your knowledge.
- **Questions:** Answer a diverse set of questions designed to challenge your intellect.
- **Friend System:** Add and manage your friends list to easily challenge them.
- **Inbox:** Exchange messages with your friends within the game.
- **Ranking:** Compete for the top rankings based on your quiz performance.
- **Score Tracking:** Your quiz results contribute to your overall score.
- **Multilingual Support:** Enjoy the game in your preferred language with built-in multilingual options.

## Technologies Used

- Backend: .NET Web API
- Frontend: React.js
- Database: Microsoft SQL Server

## Getting Started

To get started with the Quiznet locally, follow these steps:

## Backend Installation

To set up the backend of the Multiplayer Turn-Based Quiz Game, follow these steps:

1. Install the .NET SDK:
   - Download and install the [.NET SDK](https://dotnet.microsoft.com/download) for your operating system.
2. Clone this repository:
   - `git clone https://github.com/Kacwis/Quiznet.git`
3. Navigate to the backend folder:
   - `cd quiznet-api/quiznet-api`
4. Configuration Setup:
   - Create an `.env` file in the `quzinet-api/quiznet-api` directory and add the following lines with your actual values:
     ```env
     COONECTION_STRING=Server={server};Database={database_name};Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true
     SECRET_KEY={secret_key}
     ```
     - `server` - your server address.
     - `database_name` - your database name.
     - `secret_key` - created secure secret key using a mix of letters and numbers, ensuring it's sufficiently long for robust protection.
    - Save changes.
5. Build the Backend:
   - Run the following commands to restore dependencies, build, and run the backend:
     1. `dotnet restore`
     2. `dotnet build`

## Frontend Installation

1. Clone this repository: `git clone https://github.com/Kacwis/Quiznet.git`
2. Navigate to the frontend folder: `cd ./quiznet-app`
3. Install frontend dependencies: `npm install`

## Usage

1. Start the backend server:
   - Navigate to the backend folder: `quiznet-api/quiznet-api`
   - Run: `dotnet run`

2. Start the frontend development server:
   - Navigate to the frontend folder: `cd quiznet-app`
   - Run: `npm run dev`

3. Access the application in your browser at: `http://localhost:5173`

## Design Decisions

### Avatar Images Storage

I've made a conscious decision to store user avatar images on the frontend rather than the backend. This decision was driven by following considerations:

1. **Reduced Server Load:** Storing images on the frontend reduces the load on our backend server. User avatars are typically small files that can be easily loaded from the client's browser, freeing up server resources for more critical tasks.

2. **Faster User Experience:** By storing avatars on the frontend, we can leverage browser caching mechanisms to ensure quicker loading times. This enhances the overall user experience as avatars are displayed almost instantly.

3. **Pre-Defined Avatars:** In order to maintain a consistent and visually appealing experience, I've opted for a set of pre-defined avatars. Users can choose from a limited selection of avatars, reducing complexity while maintaining a unified style across the platform.

To implement this approach, avatar images are fetched from a public or CDN-hosted location, and URLs are associated with user profiles on the backend. This separation of concerns ensures a clean architecture while optimizing performance.

I believe that this decision aligns with my goal of creating a responsive and efficient multiplayer quiz game for our users.


### Frontend Technology: JavaScript vs. TypeScript

During the development of the Quiznet, I faced a significant decision: whether to use JavaScript (JS) or TypeScript (TS) for building the React frontend. After careful consideration, I chose JavaScript, and here's why:

1. **Familiarity and Learning Curve:** With my extensive experience in JavaScript, I was able to leverage my existing knowledge. This familiarity allowed me to hit the ground running and expedite the development process.

2. **Project Timeline:** Given the solo nature of this project, I needed to balance development speed with adopting new technologies. While TypeScript offers advantages, I recognized that it might have required extra time for me to learn and adapt to its strict typing system.

3. **Quick Iteration:** JavaScript's more relaxed typing system enabled me to iterate quickly and prototype different design ideas and feature implementations.

4. **Props Typing:** Despite using JavaScript, I still ensured strong typing by incorporating prop typings for React components wherever possible. This practice enhanced code clarity and provided a degree of type safety, even in a JavaScript environment.

5. **Resources:** React has a lot of JavaScript tools available, making it easy to find solutions to problems.

6. **Working Solo:** Using a language I know well helped me work efficiently and understand the whole project without switching gears.

While TypeScript has benefits like strict typing, I decided to stick with JavaScript due to my project's size and deadlines. My choice matches my aim of delivering a working, feature-rich quiz game on my own.


## Future Plans and Improvements

As the creator of the Multiplayer Turn-Based Quiz Game, I have some ideas for how to make the game even better in the future. While I can't promise exact dates or results, here are some things I'm thinking about:

### Short-Term Goals

- **UI/UX Refinements:** I plan to polish the user interface and experience to make gameplay smoother. Additionally I consider to add more animations to enhance the game's gaming feel. 

- **Bug Fixes and Performance:** I'll be actively addressing any bugs or performance issues that users encounter to ensure a seamless experience.

- **Responsive Design:** I'm working on optimizing the frontend for various devices and screen sizes to accommodate a broader audience.

- **New Language:**: I'm planning to add new language. Spanish is likely to be one of the next additions.

### Mid-Term Goals

- **Enhanced Social Features:** I'm considering improvements to the messaging system, friend interactions, and multiplayer gameplay to create a more social gaming experience.

- **Player Customization:** I'm planning to introduce more options for players to customize their profiles and avatars.

### Long-Term Vision

- **Advanced Leaderboard:** I aim to implement an advanced leaderboard system that tracks not only scores but also player achievements and milestones.

- **Community Engagement:** I'm considering features that will encourage community engagement, such as user-generated quizzes and challenges.
  
- **Authentication Provider:** I'm planning to expand the user authentication options by integrating popular social media platforms like Facebook and Instagram. This will allow users to log in and sign up using their existing accounts on these platforms, making the onboarding process more convenient.

- **Multilingual Support expansion:** I'm considering the expansion of the multilingual support by adding more languages.

Please keep in mind that these plans are tentative and may evolve over time. Feedback from users like you will play a crucial role in shaping the future direction of the Quiznet. Thank you for your support!

## Future Updates

I'm excited to share what's coming next for the Multiplayer Turn-Based Quiz Game. Here's what's in the pipeline for the near future:

- **New Language:** I'm adding Spanish to the game, making it more accessible to more players.

- **Bug Fixes and Enhancements:** I'll be fixing any issues that pop up and making things better based on your feedback.

- **Cool Animations:** I'm planning to add more animations to make the game look even cooler and more fun.

- **Mobile-Friendly Play:** I'm working on making the game work smoothly on mobile devices so you can play wherever you are.

Stay tuned for these updates!

##
This project is developed by Kacper Wiśniewski
