# DataViewer

## Overview
DataViewer is a simple web application designed to display Albums and Posts in an intuitive and user-friendly manner. The application features a Home page with navigation icons for accessing Albums and Posts.

## Features to be added
- **Home Page**
  - Displays two icons: one for **Albums** and one for **Posts** (see `Home.jpg`).
  - Allows easy navigation to respective sections.
- **Albums Page** (see `Albums.jpg`)
  - Displays a table with the following columns:
    - **Album Photo**: Displayed as a small image from a URL.
    - **Album Title**.
    - **Created By**: Shows the username of the creator.
    - **From Company**: Displays the company name of the creator.
- **Posts Page** (see `Posts.jpg`)
  - Displays multiple expandable panel items containing:
    - **Post Title**: Click to expand/collapse.
    - **Post Body**: Shown when expanded.
- **Navigation**
  - Each page includes a **Home button** for easy return to the main page.

## Architecture
The application architecture is illustrated in `Architecture.png`.

## Data Source
- Data is retrieved using **GraphQL** from: [GraphQLZero API](https://graphqlzero.almansi.me/api)
- For API usage details, refer to: [GraphQLZero Documentation](https://graphqlzero.almansi.me/)

## Installation & Usage
1. Clone the repository.
2. Install dependencies.
3. Run the application.
5. Add the Home, Albums, and Posts pages as described above.

Enjoy developing the DataViewer!

