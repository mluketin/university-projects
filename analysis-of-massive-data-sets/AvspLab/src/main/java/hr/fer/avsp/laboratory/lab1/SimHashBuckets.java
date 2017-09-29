package hr.fer.avsp.laboratory.lab1;

import java.io.*;
import java.security.NoSuchAlgorithmException;
import java.util.*;

import org.apache.commons.codec.digest.DigestUtils;

public class SimHashBuckets {

    public static void main(String[] args) throws IOException, NoSuchAlgorithmException {
        BufferedReader br = new BufferedReader(new InputStreamReader(System.in));

        int N = Integer.parseInt(br.readLine());
        byte[][] sh2D = new byte[N][128];


        for (int i = 0; i < N; i++) {
            String line = br.readLine();
            sh2D[i] = simhash128(line);
        }

        HashMap<Integer, Set<Integer>> candidates = new HashMap<>();
        for (int band = 1; band <= 8; band++) {
            HashMap<Integer, Set<Integer>> buckets = new HashMap<>();
            for (int current_id = 0; current_id < N; current_id++) {

                byte[] hash = sh2D[current_id];
                int val = hash2int(band, hash);
                Set<Integer> textsInBuckets = new HashSet<>();
                if (buckets.containsKey(val)) {
                    if (!buckets.get(val).isEmpty()) {
                        textsInBuckets = buckets.get(val);
                        for (Integer textId : textsInBuckets) {
                            if (candidates.get(current_id) == null) {
                                candidates.put(current_id, new HashSet<Integer>());
                            }
                            candidates.get(current_id).add(textId);
                            if (candidates.get(textId) == null) {
                                candidates.put(textId, new HashSet<Integer>());
                            }
                            candidates.get(textId).add(current_id);
                        }
                    }
                }
                textsInBuckets.add(current_id);
                buckets.put(val, textsInBuckets);
            }
        }

        int Q = Integer.parseInt(br.readLine());
        for (int i = 0; i < Q; i++) {
            String[] ik = br.readLine().split(" ");
            int I = Integer.parseInt(ik[0]);
            int K = Integer.parseInt(ik[1]);

            int counter = 0;

            Set<Integer> idList = candidates.get(I);
            for (int id : idList) {
                int hammDist = calculateHammDist(sh2D[I], sh2D[id]);
                if (hammDist <= K) {
                    counter += 1;
                }
            }
            String outLine = String.valueOf(counter);
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

    private static int hash2int(int pojas, byte[] hash) {
        int counter = 0;
        int sum = 0;
        for (int i = (pojas - 1) * 16; i < pojas * 16; i++) {
            sum += hash[i] * Math.pow(2, counter);
            counter++;
        }
        return sum;
    }

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
