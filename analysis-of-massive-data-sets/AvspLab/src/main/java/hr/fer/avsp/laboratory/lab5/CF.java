package hr.fer.avsp.laboratory.lab5;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.math.BigDecimal;
import java.math.RoundingMode;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;


public class CF {

    private static int[][] userGradeMatrix;
    private static double[][] userAverageMatrix;
    private static double[][] userPearsonSim;

    private static int[][] itemGradeMatrix;
    private static double[][] itemAverageMatrix;
    private static double[][] pearsonSim;
    private static int itemNumber;
    private static int userNumber;

    public static void main(String[] args) throws Exception {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        String[] pars = br.readLine().split(" ");
        itemNumber = Integer.parseInt(pars[0]);
        userNumber = Integer.parseInt(pars[1]);

        itemGradeMatrix = new int[itemNumber][userNumber];
        userGradeMatrix = new int[userNumber][itemNumber];

        itemAverageMatrix = new double[itemNumber][userNumber];
        userAverageMatrix = new double[userNumber][itemNumber];

        pearsonSim = new double[itemNumber][itemNumber];
        userPearsonSim = new double[userNumber][userNumber];


        //ITEM-ITEM grade matrix
        for (int i = 0; i < itemNumber; i++) {
            pars = br.readLine().split(" ");
            for (int j = 0; j < userNumber; j++) {
                if (pars[j].equals("X")) {
                    itemGradeMatrix[i][j] = 0;
                } else {
                    itemGradeMatrix[i][j] = Integer.parseInt(pars[j]);
                }
            }
        }

        //USER-USER grade matrix
        for (int i = 0; i < userNumber; i++) {
            for (int j = 0; j < itemNumber; j++) {
                userGradeMatrix[i][j] = itemGradeMatrix[j][i];
            }
        }

        for (int i = 0; i < itemNumber; i++) {
            itemAverageMatrix[i] = reducedByMean(itemGradeMatrix[i]);
        }

        for (int i = 0; i < userNumber; i++) {
            userAverageMatrix[i] = reducedByMean(userGradeMatrix[i]);
        }

        for (int i = 0; i < itemNumber; i++) {
            for (int j = i; j < itemNumber; j++) {
                double pearsonCoef = calculateCosineSimilarity(itemAverageMatrix[i], itemAverageMatrix[j]);
                pearsonSim[i][j] = pearsonCoef;
                pearsonSim[j][i] = pearsonCoef;
            }
        }

        for (int i = 0; i < userNumber; i++) {
            for (int j = i; j < userNumber; j++) {
                double pearsonCoef = calculateCosineSimilarity(userAverageMatrix[i], userAverageMatrix[j]);
                userPearsonSim[i][j] = pearsonCoef;
                userPearsonSim[j][i] = pearsonCoef;
            }
        }

        int Q = Integer.parseInt(br.readLine());
        for (int i = 0; i < Q; i++) {
            String line = br.readLine();
            pars = line.split(" ");

            int item = Integer.parseInt(pars[0]);
            int user = Integer.parseInt(pars[1]);

            //0 is item item, 1 is user user type
            int t = Integer.parseInt(pars[2]);

            //k is maximal cardinal number of set of similar items/users that system takes into consideration while calculating recommendations
            int k = Integer.parseInt(pars[3]);

            if (t == 0) {
                getRating(item - 1, user - 1, k, pearsonSim, itemGradeMatrix);
            } else if (t == 1) {
                getRating(user - 1, item - 1, k, userPearsonSim, userGradeMatrix);
            }
        }
        br.close();
    }

    private static void getRating(int item, int user, int k, double[][] pearsonSimilarity, int[][] matrix) {
        List<Double> similarityCopy = new ArrayList<>();
        for (int i = 0; i < pearsonSimilarity.length; i++) {
            if (i != item) {
                similarityCopy.add(pearsonSimilarity[item][i]);
            } else {
                similarityCopy.add(-5.0);
            }
        }

        List<Double> similarityForItem = new ArrayList<>(similarityCopy);

        Collections.sort(similarityCopy);
        Collections.reverse(similarityCopy);

        List<Integer> similarItems = new ArrayList<>();

        int counter = 0;
        for (int i = 0; i < similarityCopy.size(); i++) {
            if (similarityCopy.get(i) > 0 && matrix[similarityForItem.indexOf(similarityCopy.get(i))][user] > 0) {
                similarItems.add(similarityForItem.indexOf(similarityCopy.get(i)));
                counter++;
                if (counter == k) {
                    break;
                }
            }
        }


        double sumNumerator = 0;
        for (int i = 0; i < similarItems.size(); i++) {
            sumNumerator += matrix[similarItems.get(i)][user] * pearsonSimilarity[similarItems.get(i)][item];
        }

        double sumDenominator = 0;
        for (int i = 0; i < similarItems.size(); i++) {
            sumDenominator += pearsonSimilarity[similarItems.get(i)][item];
        }

        double result = sumNumerator / sumDenominator;
        writeDouble(result);
    }

    private static void writeDouble(double d) {
        DecimalFormat df = new DecimalFormat("0.000");
        BigDecimal bd = new BigDecimal(d);
        BigDecimal res = bd.setScale(3, RoundingMode.HALF_UP);
        System.out.println(df.format(res).replace(",", "."));
    }

    private static double[] reducedByMean(int[] vector) {
        double sum = 0;
        int counter = 0;
        double[] newVector = new double[vector.length];

        for (int i = 0; i < vector.length; i++) {
            if (vector[i] != 0) {
                sum += vector[i];
                counter++;
            } else {
                newVector[i] = 0;
            }
        }
        double avrg = sum / counter;

        for (int i = 0; i < vector.length; i++) {
            if (vector[i] != 0) {
                newVector[i] = vector[i] - avrg;
            } else {
                newVector[i] = 0;
            }
        }
        return newVector;
    }

    private static double calculateCosineSimilarity(double[] vecA, double[] vecB) {
        double dotProduct = dotProduct(vecA, vecB);
        double magnitudeOfA = magnitude(vecA);
        double magnitudeOfB = magnitude(vecB);

        return dotProduct / (magnitudeOfA * magnitudeOfB);
    }

    private static double dotProduct(double[] vecA, double[] vecB) {
        double dotProduct = 0;
        for (int i = 0; i < vecA.length; i++) {
            dotProduct += (vecA[i] * vecB[i]);
        }
        return dotProduct;
    }

    private static double magnitude(double[] vector) {
        return Math.sqrt(dotProduct(vector, vector));
    }
}
