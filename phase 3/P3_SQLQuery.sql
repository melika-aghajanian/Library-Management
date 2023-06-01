CREATE TABLE Library
(
	ID INT PRIMARY KEY,
	LIB_Name NVARCHAR(45),
	LIB_Address NVARCHAR(100),
	Members_Count INT
);

INSERT INTO Library (ID, LIB_Name, LIB_Address, Members_Count)
VALUES
  (1, N'کتابخانه یک', N'ادرس یک', 100),
  (2, N'کتابخانه دو', N'ادرس دو', 150),
  (3, N'کتابخانه سه', N'ادرس سه', 200),
  (4, N'کتابخانه چهار', N'ادرس چهار', 100),
  (5, N'کتابخانه پنج', N'ادرس پنج', 150);

/**************************************************************************************************************************/
CREATE TABLE Librarian
(
	ID INT PRIMARY KEY ,
	Personnal_ID INT,
	L_Name NVARCHAR(45) ,
	L_Phone VARCHAR(45),
	Head_ID INT FOREIGN KEY REFEReNCES Librarian(ID)
);

INSERT INTO Librarian (ID, Personnal_ID, L_Name, L_Phone, Head_ID)
VALUES
  (1, 1001, N'زهرا امینی', '1111111111', NULL),
  (2, 1002, N'شیما منانی', '2222222222', 1),
  (3, 1003, N'علی ارشی', '3333333333', 1),
  (4, 1004, N'مسیح آقاجانیان', '4444444444', 1),
  (5, 1005, N'مریم موسوی', '5555555555', 1);

/**************************************************************************************************************************/

CREATE TABLE Section 
(
	ID INT NOT NULL PRIMARY KEY ,
	S_Name NVARCHAR(45) ,
	Manager INT FOREIGN KEY REFERENCES Librarian(ID)
);

INSERT INTO Section (ID, S_Name, Manager)
VALUES
  (1, N'بخش یک', 1),
  (2, N'بخش دو', 2),
  (3, N'بخش سه', 3),
  (4, N'بخش چهار', 4),
  (5, N'بخش پنج', 5);

/**************************************************************************************************************************/

CREATE TABLE Writer
(
	ID INT PRIMARY KEY,
	W_Name NVARCHAR(45),
	W_Phone VARCHAR(45)
);

INSERT INTO Writer (ID, W_Name, W_Phone)
VALUES
  (1, N'شقایق شهبازی', '1111111111'),
  (2, N'نیوشا برقعیان', '2222222222'),
  (3, N'محمد شیرانی', '3333333333'),
  (4, N'فاطمه شریفی', '4444444444'),
  (5, N'عرفان رفیعایی', '5555555555');

/**************************************************************************************************************************/

CREATE TABLE Translator
(
	ID INT PRIMARY KEY,
	T_Name NVARCHAR(45),
	T_Phone VARCHAR(45)
);

INSERT INTO Translator (ID, T_Name, T_Phone)
VALUES
  (1, N'مینا علینقیان', '1111111111'),
  (2, N'حسین لادانی', '2222222222'),
  (3, N'مهرداد عباسی', '3333333333'),
  (4, N'صبا کیانی', '4444444444'),
  (5, N'مینو علیخانی', '5555555555');

/**************************************************************************************************************************/

CREATE TABLE Member
(
	ID INT PRIMARY KEY,
	M_Name NVARCHAR(45),
	M_Phone VARCHAR(45),
	M_Address NVARCHAR(100),
	Membership_Number INT,
	Registery_Date DATE,
	Reserved_Book INT,
	M_Image IMAGE
);

ALTER TABLE Member
ADD CONSTRAINT M_D DEFAULT
0x For M_Image

INSERT INTO Member (ID, M_Name, M_Phone, M_Address, Membership_Number, Registery_Date, Reserved_Book)
VALUES
  (1, N'شقایق باقری', '1111111111', N'ادرس یک', 11111, '2023-05-01', 2 ),
  (2, N'شاهین باقری', '2222222222', N'ادرس دو' , 22222, '2023-04-15', 1),
  (3, N'آیسان رفیعایی', '3333333333', N'ادرس سه' , 33333, '2023-03-20', 0),
  (4, N'فرهان علینقیان', '4444444444', N'ادرس چهار' , 44444, '2023-05-10', 3),
  (5, N'مهدی مسائلی', '5555555555', N'ادرس پنج' , 55555, '2023-02-28', 1);
  
/**************************************************************************************************************************/

CREATE TABLE TakePart
(
	ID INT PRIMARY KEY,
	Library_ID INT FOREIGN KEY REFERENCES Library(ID),
	Member_ID INT FOREIGN KEY REFERENCES Member(ID)
);

INSERT INTO TakePart (ID, Library_ID, Member_ID)
VALUES
  (1, 1, 1),
  (2, 2, 2),
  (3, 3, 3),
  (4, 4, 4),
  (5, 5, 5);

/**************************************************************************************************************************/

CREATE TABLE Book
(
	ID INT PRIMARY KEY,
	B_Name NVARCHAR(45),
	Publisher_Name NVARCHAR(45),
	Publish_Date DATE,
	B_Status NVARCHAR(45), /* reserved , available , borrowed*/
	Librarian_ID INT FOREIGN KEY REFERENCES Librarian(ID),
	Section_ID INT FOREIGN KEY REFERENCES Section(ID),
	Writer_ID INT FOREIGN KEY REFERENCES Writer(ID),
	Translator_ID INT FOREIGN KEY REFERENCES Translator(ID)
);

INSERT INTO Book (ID, B_Name, Publisher_Name, Publish_Date, B_Status, Librarian_ID, Section_ID, Writer_ID, Translator_ID)
VALUES
  (1, N'کتاب یک', N'انتشارات یک', '2022-10-15', N'امانت', 1, 1, 1, 1),
  (2, N'کتاب دو', N'انتشارات دو', '2023-01-20', N'امانت', 2, 2, 2, 2),
  (3, N'کتاب سه', N'انتشارات سه', '2023-03-05', N'امانت', 3, 3, 3, 3),
  (4, N'کتاب چهار', N'انتشارات چهار', '2022-09-10', N'رزرو',4, 4, 4, 4),
  (5, N'کتاب پنج', N'انتشارات پنج', '2023-04-30', N'رزرو', 5, 5, 5, 5);
  

/**************************************************************************************************************************/

CREATE TABLE Request 
(
	ID INT primary key,
	Member_ID INT FOREIGN KEY REFERENCES Member(ID),
	Book_ID INT FOREIGN KEY REFERENCES Book(ID)
);

INSERT INTO Request (ID, Member_ID , Book_ID)
VALUES
  (1, 1, 1),
  (2, 2, 2),
  (3, 3, 3),
  (4, 4, 4),
  (5, 5, 5);

/**************************************************************************************************************************/

CREATE TABLE Borrow
(
	ID INT PRIMARY KEY,
	Librarian_ID INT FOREIGN KEY REFERENCES Librarian(ID),
	Member_ID INT FOREIGN KEY REFERENCES Member(ID),
	Book_ID INT FOREIGN KEY REFERENCES Book(ID),
	Borrow_Date DATE,
	Return_Date DATE,
	Forfeit FLOAT,
	confirm INT
);

INSERT INTO Borrow (ID, Librarian_ID, Member_ID, Book_ID, Borrow_Date, Return_Date, Forfeit, confirm)
VALUES
  (1, 1, 1, 1, '2023-05-01', '2023-05-15', 0.0, 1),
  (2, 2, 2, 2, '2023-05-05', '2023-05-20', 0.0, 1),
  (3, 3, 3, 3, '2023-05-10', '2023-05-25', 0.0, 1),
  (4, 4, 4, 4, '2023-05-15', '2023-05-30', 0.0, 1),
  (5, 5, 5, 5, '2023-05-20', '2023-06-04', 0.0, 1);


