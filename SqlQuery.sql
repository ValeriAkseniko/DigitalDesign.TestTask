--1.Сотрудника с максимальной заработной платой.
-- В данном случае затрачивается ресорс на сортировку.
SELECT TOP(1) 
   Id AS EmployeeID,
   Name AS EmployeeName,
   Salary
FROM Employee
ORDER BY Salary DESC

-- Здесь же затраты уходят на подзапрос.
SELECT TOP(1) 
   Id AS EmployeeID,
   Name AS EmployeeName,
   Salary
FROM Employee
WHERE Salary = (SELECT MAX(Salary) FROM Employee)


--2.Отдел, с самой высокой заработной платой между сотрудниками.
--Отдел с самой высокой зарплатой у сотрудника
SELECT TOP(1) 
  Department.Id AS DepartmentId, 
  Department.Name AS DepartmentName,
  Employee.Salary AS MaxSalary
FROM Department
JOIN Employee ON Department.Id = Employee.DepartmentId
ORDER BY Employee.Salary DESC

--Отдел с самой высокой средней зарплатой у сотрудников
SELECT TOP(1) 
   AVG(Salary) AS SumSalary,
   Department.Id AS DepartmentId, 
   Department.Name AS DepartmentName
FROM Employee
JOIN Department ON Employee.DepartmentId = Department.Id
GROUP BY Department.Id , Department.Name
ORDER BY SumSalary DESC


--3.Отдел, с максимальной суммарной зарплатой сотрудников. 
SELECT TOP(1) 
   Sum(Salary) AS SumSalary,
   Department.Id AS DepartmentId, 
   Department.Name AS DepartmentName
FROM Employee
JOIN Department ON Employee.DepartmentId = Department.Id
GROUP BY Department.Id , Department.Name
ORDER BY SumSalary DESC

--4.
--Сотрудника, чье имя начинается на «Р» и заканчивается на «н».
SELECT TOP(1) 
   Employee.Id AS EmployeeId,
   Employee.Name AS EmployeeName
FROM Employee
WHERE Employee.Name LIKE 'Р%н'

--Имя начинается на «Р».
SELECT TOP(1) 
   Employee.Id AS EmployeeId,
   Employee.Name AS EmployeeName
FROM Employee
WHERE Employee.Name LIKE 'Р_'

--Имя заканчивается на «н».
SELECT TOP(1) 
   Employee.Id AS EmployeeId,
   Employee.Name AS EmployeeName
FROM Employee
WHERE Employee.Name LIKE '%н'

