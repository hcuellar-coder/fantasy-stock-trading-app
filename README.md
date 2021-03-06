## Fantasy Stock Trading App
The application utilizes IEX Cloud API and was built using ReactJS (Frontend), C# with ASP.NET Core (Backend) using Nhibernate with Fluent along with PostgreSQL and styled with Bootstrap 4.

Run Application [Here](http://fantasystocktrader.us-east-2.elasticbeanstalk.com/)

<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Home.PNG">
</p>

## Summary
This web application utilizes IEX Cloud API to retrieve and utilize stock information to simulate a stock trading application, using React-Bootstrap for styling and responsiveness. ReactJS was used to develop the frontend of the web application. While the backend uses C# with ASP.NET Core for RESTFUL WebAPI and Internal WebAPI calls. An ORM, Nhibernate along with Fluent, was used to map objects models to data models. As well as perform query operations to access data from the PostgreSQL database. The PostgreSQL is being used to handle User, Account, Transaction and Holding information. Session storage and React Context Hooks were used to store user, account, holding and other information to provide a smoother user experience. 

#### Unit Testing 
This application also implements unit testing. Using NUnit as the unit-testing framework and NSubstitute to mock libraries. It was exciting to learn about unit testing and understand the potential of a TDD approach to programming.


### Login and Signup Page
This web application begins with a simple login / sign-up that is used to create a user account. If you have a user account, go ahead and log in. If you are new to the web application, you will need to sign up. Upon creation of the user account, the user will be allotted $100,000, to start buying stocks to add to their portfolio. Upon logging on there will be an automatic update to one's holdings which will also update the portfolio balance.

<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Login.PNG">
<br>
<br>
<img align="center"  width="600" height="500" src="./readme-images/SignUp.PNG">
</p>

### Home Page
The home page is where the user will arrive at once they have logged in. Here the user will be given descriptions of the functionality of the summary and report pages, and what they have in store for the user.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Home.PNG">
</p>

### Summary Page
The summary page is where the user will be able to purchase stocks, while also keeping track of the amount of funds the user has in their balance. As you purchase more stocks the balance is reduced, the portfolio amount increases and vice versa. The summary page contains 3 tabs, all of which can be used to purchase stocks. The My Holdings Tab is the only tab where selling is possible.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Summary.PNG">
</p>

#### Search
Here the user can search for a specific stock symbol, which will return the latest price and the change from the last closing price. This will give you an idea on how the stock is doing in a more immediate time frame. 
The buy button will open the transaction modal prompting the user for how many stocks they would like to purchase.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Summary-Search.PNG">
</p>

#### Most Active
The Most Active tab contains the top 10 most active stocks, provided by the IEX Cloud API, that you can purchase.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Summary-MostActive.PNG">
</p>

#### My Holdings
The Holdings tab will contain the user's holdings, all the stocks the user has purchased. From this tab, the user can purchase, or sell stock. The stocks are automatically updated upon purchase or selling, with the balances reflecting the user's actions.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Summary-MyHoldings.PNG">
</p>

#### Transaction Modal
The Transaction modal is the key to trading stocks. With the modal having the capability to purchase or sell stocks that are either in the user's holdings, the most active stock list or the from the search tab.
<p align="center">
<img align="center"  width="600" height="400" src="./readme-images/TransactionModal.PNG">
</p>


### Report Page
The report page is where the user will be able to view 1 month's worth of stock prices in either the Search, Most Active, My Holdings Tabs. The final tab, Portfolio, is used to display the user's portfolio balance split by stock into some easy to read graphs.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Report.PNG">
</p>


#### Search
In the search tab the user can search for a specific stock symbol to view 1 month's worth of that stocks prices.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Report-Search-Graph.PNG">
</p>

#### Most Active
In the Most Active tab, the user can view 1 month's worth of stock prices for the top 10 most active stocks provided by the IEX Cloud API. 
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Report-MostActive-Graph.PNG">
</p>

#### My Holdings
In My Holdings tab the user will be able to view 1 months' worth of stock prices for any of the holdings that the user has purchased.  will contain the user's holdings, all the stocks the user has purchased. From this tab, the user can view 1 month's worth of stock prices.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Report-MyHoldings-Graph.PNG">
</p>

### My Portfolio
In My Portfolio Tab the user's portfolio balance is split by stock into some easy to read graphs. This is a great way to visualize which stock most of your portfolio balance is allocated to.
<p align="center">
<img align="center"  width="600" height="500" src="./readme-images/Report-Portfolio-Graph.PNG">
</p>

### Author's Note
Creating this web application strengthened my knowledge of how to integrate a ReactJS Front End with C# in a ASP.NET Core Back End for RESTful web API calls and Internal API calls. This application was a lot of fun because there were a lot of firsts for me. It was my first time using an ORM, Nhibernate and fluent, to map my object models to data models and query the database for information as an API endpoint with ASP.NET Core. It was also my first experience deploying a web application with a database using Microsoft Azure. Lastly, it was my first time working with NUnit and NSubstitute for unit testing my services. I had a wonderful experience creating the application and working through the intricacies of integrating the different technologies together. I also made sure that the styling of the web application was responsive and would work well on a Mobile Device. I hope you enjoy the using my application!

## Author
Heriberto Cuellar – Full Stack Software Developer - [Website](https://heribertocuellar.com) | [LinkedIn](https://www.linkedin.com/in/heriberto-cuellar/)
