## Fantasy Stock Trading App

The application utilizes IEX Cloude API and was built using ReactJS (Frontend), C# with ASP.NET Core (Backend) and styled with Bootstrap 4

Run Application [Here](https://stocktradingapp-hcuellar.azurewebsites.net/)

<!-- <p align="center">
<img align="center"  width="600" height="400" src="./images/home.png">
</p> -->

## Summary
This web application utilizes IEX Cloude API to retrieve and utilize stock information to simulate a stock trading fantasy game, using Bootstrap for styling and responsiveness. ReactJS was used to develop the frontend of the web application. While the backend uses C# with ASP.NET Core for RESTFUL WebAPI and Internal WebAPI calls. Session storage and React Context Hooks were used to store user, account, holding and other information to provide a smoother user experience.


###Login and Signup Page

This web application begins with a simple login / sign-up that is used to create a user account. If you have a user account, go ahead and log in. If you are new to the web application you will need to sign up. Upon creation of the user account, the user will be alloted $100,000, to start buying stocks to add to their portfolio. Upon logging on there will be an automatic update to ones holdings which will also update the portfolio balance.

###Home Page

The home page is the offical landing page of the application. The user will arrive at this page once they have logged in. Here you will be given a simple instruction on what the next two pages have in store for the user.

###Summary Page
The summary page is where the user will be able to purchase stocks. While also keeping track of the amount of money the user has in their balance. As you purchase more stocks the balance is reduced, the portfolio amount increases and vice versa.

<b>To purchase stocks, the user has 3 options</b>:

####Search
Here the user can search for a specific stock symbol, which will return the latest price and the change from the last closing price. This will give you an idea on how the stock is doing in the immediate time frame. 
####--Image--
The buy button will open a modal prompting the user for how many stocks they would like to purchase.
####--Image--

####Most Active

####My Holdings

###Report Page

