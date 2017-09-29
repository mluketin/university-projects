package hr.fer.avsp.laboratory.lab1;

import java.io.*;
import java.security.NoSuchAlgorithmException;

import org.apache.commons.codec.digest.DigestUtils;

public class SimHash {

    public static void main(String[] args) throws NoSuchAlgorithmException, IOException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        int N = Integer.parseInt(br.readLine());
        byte[][] sh2D = new byte[N][128];
        int[] diff = new int[128]; //for each Hamm distance contains num of lines

        int[][] dif2D = new int[N][128];

        for (int i = 0; i < N; i++) {
            String line = br.readLine();
            sh2D[i] = simhash128(line);
        }

        for (int i = 0; i < N; i++) {
            for (int j = 0; j < N; j++) {
                if (i != j) {
                    int hd = calculateHammDist(sh2D[i], sh2D[j]);
                    dif2D[i][hd] += 1;
                }
            }
        }

        int Q = Integer.parseInt(br.readLine());
        for (int i = 0; i < Q; i++) {
            String[] ik = br.readLine().split(" ");
            int I = Integer.parseInt(ik[0]);
            int K = Integer.parseInt(ik[1]);

            int sum = 0;
            for (int j = 0; j <= K; j++) {
                sum += dif2D[I][j];
            }
            String outLine = String.valueOf(sum);
            System.out.println(outLine);
        }

        br.close();
    }

    public static int calculateHammDist(byte[] bytes1, byte[] bytes2) {
        int counter = 0;
        for (int i = 0; i < 128; i++) {
            if (bytes1[i] != bytes2[i]) {
                counter += 1;
            }
        }
        return counter;
    }

    /**
     * Returns byte[] of size 128 representing 128bit hash
     */
    public static byte[] simhash128(String text) throws UnsupportedEncodingException, NoSuchAlgorithmException {
        String[] words = text.split(" ");
        int[] sh = new int[128];

        for (String word : words) {
            byte[] hash = DigestUtils.md5(word);
            for (int i = 0; i < hash.length; i++) {
                for (int j = 7; j >= 0; j--) {
                    int bit = getBit(hash[i], j);
                    if (bit == 1) {
                        sh[i * 8 + 7 - j] += 1;
                    } else {
                        sh[i * 8 + 7 - j] -= 1;
                    }
                }
            }
        }

        byte[] byteSh = new byte[128];
        for (int i = 0; i < sh.length; i++) {
            if (sh[i] >= 0) {
                byteSh[i] = 1;
            } else {
                byteSh[i] = 0;
            }
        }
        return byteSh;
    }

    public static int getBit(byte b, int position) {
        return (b >> position) & 1;
    }
}
