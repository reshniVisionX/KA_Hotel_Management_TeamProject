   Rules to follow while implement the BookMyTable frontend

1. Don't change the initial structure created
2. All the api calls must be correctly placed in the respective files in the service folder
2. Don't use "any" anywhere in the code. all must be specified either as a type or interface.
3. Interface must be placed in the "types" folder
4. All the colors must use the global color in the "App.css" 
5. No inline stylings must be mentioned in the .tsx component must be placed in the styles folder under the respective folder.
6. For all the success and error response need to display to the user from the api call must use only the Toast.tsx under the Utils folder
7. For the user data where-ever required use the authSlice
8. Fetch from slice for notification,restaurant and menulist data for the customer.And use these data from the store if already available
9. The initial coding structure must be maintained at any case
10. For all the forms input use the dynamic validation like the one i mentioned in the register page
11. For redux implementation the api call must be in the "Thunk" folder
12. Follow the structure of the "Service" folder api implementation. 
13. use handleAxiosError only at the  service layer/UI component /thunk  as per requirement
14. All the role based routings must be inside the App.tsx
15. In the component to display the data from the api use the types -> "ApiSuccessResponse" and "ApiErrorResponse"
16. use   const error = err as AppError;  in the components catch block

