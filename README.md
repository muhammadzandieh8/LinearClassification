# LinearClassification
Separation of two data classes by distance measurement by machine learning



In the Tab data , there are two sections, Guassian1 and Guassian2, where data can be generated in different coordinates
In Summary Tab, Weights and a and InputData(X,Y) and WeightState can be displayed in the table for every inputed Data 

T = Target - The correct amount of categorized data for each data
 Contains the values ​​0 and 1, each of which specifies the data class


A
Is a value that indicates which class the entered data belongs to

How to categorize input data:
The initial weight can be selected in 3 ways
 
1: randomly
2: The user enters the weight
3: With the first input data

To determine which side of the line this point is on
First, given that the data is perpendicular to the divider line, we obtain the point on the divider line. Then, given the distance between the point and the origin and the input data from the origin, we can identify which class it belongs to. After the training is over

Because we are working with the line equation, there may be two equations that answer the problem, and both of these answers are correct, but in the coordinates there is only one answer that falls between the two classes of data, then at the end of the calculation The distance between the two classes and the line equation can solve this problem.



How to update weights:
If T = 1 and a = 0, our weight is underestimated and must be added to the input data.


If T = 0 is a = 1, our weight is a little high and should be deducted from the input data.

If t is equal to a. Our weight is right

Instead of working with variables t and a, you can use a variable called e, which is as follows
e = t - a

W (new) = w (old) + e * p

Or otherwise
W (new) = w (old) + (t-a) * p


