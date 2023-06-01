
/*1*/
/*نمایش مشخصات کتابخانه a */
SELECT * 
FROM Library

/*نمایش نام هر کتاب داخل بخش مربوطه در کتابخانه b*/
SELECT S_Name , B_Name
FROM Book JOIN Section
ON Section.ID = Book.Section_ID

/*جست و جوی کتاب طبق نام کتاب یا نام نویسنده یا هردو c*/
DECLARE @BNAME VARCHAR(45) ;
DECLARE @WNAME VARCHAR(45) ;

SELECT Book. *
FROM Book JOIN Writer
ON Book.Writer_ID = Writer.ID
WHERE Book.B_Name = @BNAME OR Writer.W_Name = @WNAME 


/*2*/

DECLARE @MemberNum int ;

/*نمایش مشخصات کتابخانه a*/
SELECT * 
FROM Library
WHERE @MemberNum in (
SELECT Membership_Number
FROM Member
)

/*نمایش نام هر کتاب داخل بخش مربوطه در کتابخانه b*/
SELECT S_Name , B_Name
FROM Book JOIN Section
ON Section.ID = Book.Section_ID
WHERE @MemberNum in (
SELECT Membership_Number
FROM Member
)

/*جست و جوی کتاب با استفاده از تمامی ویژگی ها c*/
DECLARE @B_NAME VARCHAR(45) ;
DECLARE @W_ID INT ;
DECLARE @T_ID INT ;
DECLARE @S_ID INT ;
DECLARE @P_NAME VARCHAR(45) ;
DECLARE @P_DATE DATE ;


SELECT Book. *
FROM Book
WHERE ( Book.B_Name = @B_NAME ) OR (Book.Writer_ID = @W_ID) OR (Book.Translator_ID = @T_ID) 
OR (Book.Section_ID = @S_ID)  OR (Book.Publisher_Name = @P_NAME) OR (Book.Publish_Date = @P_DATE) 
and @MemberNum in (
SELECT Membership_Number
FROM Member
)

/*مشاهده اطلاعات عضو هنگام لاگین به همراه نام کتاب های امانت گرفته شده d*/
SELECT Member. * , Borrow.Book_ID
FROM Member JOIN Borrow
ON Borrow.Member_ID = Member_ID 
WHERE @MemberNum in (
SELECT Membership_Number
FROM Member
)

/*امانت گرفتن کتاب e*/
DECLARE @Borrow_ID INT ;
DECLARE @Borrow_LibrarianID INT  ;
DECLARE @Borrow_MemberID INT ;
DECLARE @Borrow_BookID INT ;
DECLARE @Borrow_Date DATE ;
DECLARE @Borrow_ReturnDate DATE ;


INSERT INTO Borrow VALUES (@Borrow_ID , @Borrow_LibrarianID , @Borrow_MemberID , @Borrow_Date , @Borrow_ReturnDate , 0 , 1) ;

/*رزرو کتابی که به امانت گرفته شده است f*/
declare @MemberNum INT;
update Book set B_Status='reserved'
where Book.B_Status = 'borrowed' and @MemberNum in (SELECT Membership_Number
                        FROM Member)

/*محاسبه جریمه به ازای دیرکرد در تحویل کتاب g*/
DECLARE @BID INT ;

UPDATE Borrow SET Forfeit = 5000 * ( DATEDIFF(day, Return_Date , GETDATE()))
WHERE Borrow.Book_ID = @BID 
and @MemberNum in (
SELECT Membership_Number
FROM Member
)

/*3*/
/*مشاهده اطلاعات خود ، بخش مربوطه و وضعیت مدیریت بخش مربوطه هنگام لاگین a*/
DECLARE @PersonalID INT ;

SELECT Librarian. * , Section. *
FROM Librarian JOIN Section
ON Section.Manager = Librarian.Head_ID 
WHERE @PersonalID in (
SELECT Personnal_ID
FROM Librarian
)

/*مشاهده اطلاعات مسئولین و کتاب های بخش مربوطه b*/
SELECT Librarian. * , Book. * 
FROM ( Librarian JOIN Section 
ON Librarian.Head_ID = Section.Manager) JOIN Book	
ON book.Section_ID = Section.ID 
WHERE Librarian.Head_ID = ( select Librarian.Head_ID from Librarian where Librarian.Personnal_ID = @PersonalID )
and @PersonalID in (
SELECT Personnal_ID
FROM Librarian
)


/*حذف یا افزودن عضو جدید یا یک کتاب جدید c*/
DECLARE @NMemebrID INT ;
DECLARE @NMemebrName VARCHAR(45) ;
DECLARE @NMemberNum	 INT ;

INSERT INTO Member VALUES (@NMemebrID , @NMemebrName , null , null , @NMemberNum , null , null , null) ;
DELETE FROM Member WHERE Member.Membership_Number = @MemberNum ;

DECLARE @NBookID INT ;
DECLARE @NBookName VARCHAR(45) ;
DECLARE @NBookPublishName VARCHAR(45) ;
DECLARE @NBookLibrarianID INT ;
DECLARE @NBookSectionID INT ;
DECLARE @NBookWriteID INT ;
DECLARE @NBookTranslatorID INT ;

INSERT INTO Book VALUES (@NBookID , @NBookName , @NBookPublishName , null , 'available' , @NBookLibrarianID , @NBookSectionID , @NBookWriteID , @NBookTranslatorID) ;
DELETE FROM Book WHERE book.ID = @NBookID and Book.Section_ID = @NBookSectionID ;

/*تایید را رد امانت d*/
UPDATE Borrow SET confirm = 1 ;

/*حذف کتاب ها با تاریخ قدیمی با امانت کمتر از 5 بار e*/
DELETE FROM Book
WHERE ID IN (
  SELECT Book_ID
  FROM Borrow
  WHERE Book_ID IN (
    SELECT ID
    FROM Book
    WHERE Publish_Date < '1990-01-01'
  )
  GROUP BY Book_ID
  HAVING COUNT(*) < 5
);


/*4*/
/*انتخاب مدیر از بین مسئولینی که در سال گذشته بیشترین امانت را ثبت کرده اند a*/
SELECT Borrow.Librarian_ID
FROM Borrow
WHERE DATEDIFF(day, Borrow_Date , GETDATE()) <= 365
GROUP BY Borrow.Librarian_ID
HAVING COUNT(Borrow_Date) = ( SELECT MAX(NMD)
								FROM (SELECT Borrow.Librarian_ID , COUNT(*) AS NMD
										FROM Borrow
										GROUP BY Borrow.Librarian_ID) AS NMD2 )

/*نمایش مدیر مسئولین b*/
SELECT H.ID, H.L_Name
FROM Librarian L
JOIN Librarian H ON L.Head_ID = H.ID;


/*مدیر، مسئولین را مشاهده می کند c*/
DECLARE @HeadID INT ;

SELECT Librarian.ID
FROM Librarian

/*مدیر می تواند تمامی اعضا و کتاب ها را مشاهده کند d*/
SELECT Member. * , Book. *
FROM Member , Book

/*مدیر می تواند مشاهده کند کدام بخش بیشترین تعداد امانت کتاب را دارد e*/
SELECT Book.Section_ID
FROM Borrow JOIN Book
ON Book.ID = Borrow.Book_ID
GROUP BY Borrow.Librarian_ID
HAVING COUNT(Borrow.Book_ID) = ( SELECT MAX(NMD)
								FROM (SELECT Book.Section_ID , COUNT(*) AS NMD
										FROM Book JOIN Borrow
										ON Borrow.Book_ID = Book.ID 
										GROUP BY Book.Section_ID) AS NMD2 )


/*مدیر می تواند مشاهده کند کدام عضو بیشترین فعالیت را دارد f*/
SELECT Borrow.Member_ID
FROM Borrow
WHERE DATEDIFF(day, Borrow_Date , GETDATE()) <= 365
GROUP BY Borrow.Member_ID
HAVING COUNT(Borrow_Date) = ( SELECT MAX(NMD)
								FROM (SELECT Borrow.Member_ID , COUNT(*) AS NMD
										FROM Borrow
										GROUP BY Borrow.Member_ID) AS NMD2 )