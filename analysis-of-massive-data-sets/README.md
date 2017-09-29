Analysis of massive data sets (https://www.fer.unizg.hr/en/course/aomds)

Laboratory exercises for the course. 
All five exercises are in one project.
Each class solvese separate problem.


LAB1

Task of lab1 is to hash texts using Simhash algorithm.
Simhash can identify similar texts.

SimHash:

Input:
N (N <= 1000) - number of texts
n lines of text (each ends in \n)
Q (Q <= 100) - number of queries 
q queries 

Each query has two integers separated by space (​I, K; 0 <= I <= N-1 i 0 <= K <= 31​)
For each query, program has to generate total number of texts whose hashes are different in [0, K] bits from the I-th hash (text in I-th input row).
Hash difference is calculated using Hammings distance.

Output:
q lines, each consisting of one integer



SimHashBuckets

Similar to first task, but LSH algorithm is used (​Locality Sensitive Hashing).

Input is the same as input for SimHash.
Following limitations apply:
- N <= 10^5
- Q <= 10^5
- 0 <= I <= N-1
- 0 <= K <= 31​


LAB2

Park-Chen-Yu algorithm (PCY)
Task is to find similar sets of items.
We have buckets and each bucket has items.
One item type can be found in multiple buckets (one item type cannot be found twice in one bucket).

input is:
- N - number of buckets
- s - threshold
- b - number of available compartments for hashing
- n lines of buckets, each bucket has list of integers (items)

N
s
b
bucket 1
bucekt 2
...
bucket N

output is:
- A - sum of candidates of frequent items which A-priori algorithm would count
- P - sum of pairs that PCY counts
- n lines of frequent pairs (only sum of repetitions is printed) (sort descending)

A
P
X1
X2
...
Xn


LAB3

Datar-Gionis-Indyk-Motwani (DGIM) algorithm for counting bits in stream

input:
- N - size of window
- bits - sequence of 0 and 1, maximum size of 80 charactes in line (easier to read)
- query - query format is "q <k>" where k is size of window for query (1 <= k <= N)

N
bits or query
...
bits or query

output is number of ones for a query

example

input: 
	100 
	1010101101 
	1110101011 
	q 20 
	1000010010 
	q 3 

output:
	11
	0
	
	
LAB4

NodeRank - calculate rank for all nodes in graph (no dead ends)

input:
- n Beta - n is number of nodes (1 <= n <= 10^5), beta is probability of random walker following graph (1-Beta is probability of teleportation)
- n lines that describe edges in graph
- Q - number of queries
- q lines of queries, each query is two integers, index of node and iteration

output:
- value of rank of requested node in requested iteration


ClosestBlackNode

Graph consists of black and white nodes.
For each white node calculate distance to nearest black node.
(if white node has 2 black nodes that are on same distance, take black node with lower index)

input:
- n e, number of nodes (1 to 10^5) and number of edges (e <= 2.5*n)
- n lines that describe type of node (0 or 1) (0 is white, 1 is black)
- e lines each consists of two integers (each int is index of node) that are connected

output:
- n lines: format "b dist" => b is index of black node closest to i-th white node and dist is distance

b0 dist0
b1 dist1
...
bn-1 distn-1



LAB5

Collaborative Filtering for user-user and item-item



















