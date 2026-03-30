📚 Book Management System API

A backend system designed to manage all core operations of a digital library, including books, authors, users, borrowing processes, fines, and user interactions.
The system provides a complete solution for handling the lifecycle of books from creation and categorization to borrowing, returning, and reviewing while ensuring secure authentication and efficient data management.
It is built using Clean Architecture, which separates concerns into independent layers, making the system maintainable, testable, and easy to extend.

💡 Key Capabilities
- Manage books with advanced features such as search, pagination, and popular books tracking
- Handle author and category management with full CRUD operations
- Implement a complete borrowing workflow (borrow → approve → return)
- Manage fines and allow users to pay them
- Provide a review system for users to rate and comment on books
- Send and manage notifications for important events
- Provide secure authentication and authorization using JWT and Identity

⚡ Performance & Scalability
The system is optimized using:
- Redis caching to improve response time
- Entity Framework Core (Code First) for database management
- RabbitMQ

 🚀 Features 
- Authentication & Authorization using JWT
- User Profile Management
- Book Management (CRUD + Search + Popular Books)
- Author Management
- Category Management
- Borrowing System (Borrow / Approve / Return)
- Fine System (Pay fines)
- Review System
- Notifications System
- Globalization and Localization 
