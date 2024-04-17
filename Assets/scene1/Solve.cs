using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;
using System.IO;
using System.Globalization;


namespace program
{

    public class Solve : MonoBehaviour
    {   
        const int NUM_CORNERS = 8;
        const int NUM_EDGES = 12;
        const int NUM_CO = 2187;
        const int NUM_EO = 2048;
        const int NUM_E_COMBINATIONS = 495;
        const int NUM_CP = 40320;
        const int NUM_UD_EP = 40320;
        const int NUM_E_EP = 24;
        const int MOVE_NAMES_LEN = 18;
        const int MOVE_NAMES_PH2_LEN = 10;

        public class Cube
        {
            public int[] corner_permutation = new int[8];
            public int[] corner_orientation = new int[8];
            public int[] edge_permutation = new int[12];
            public int[] edge_orientation = new int[12];

            public Cube(int[] cp, int[] co, int[] ep, int[] eo)
            {
                for (int i = 0; i < 8; i++) { corner_permutation[i] = cp[i]; }
                for (int i = 0; i < 8; i++) { corner_orientation[i] = co[i]; }
                for (int i = 0; i < 12; i++) { edge_permutation[i] = ep[i]; }
                for (int i = 0; i < 12; i++) { edge_orientation[i] = eo[i]; }
            }

            public Cube move_cube(Cube move)
            {
                int[] new_cp = new int[8];
                int[] new_co = new int[8];
                int[] new_ep = new int[12];
                int[] new_eo = new int[12];

                for (int i = 0; i < 8; i++) { new_cp[i] = corner_permutation[move.corner_permutation[i]]; }
                for (int i = 0; i < 8; i++) { new_co[i] = ((corner_orientation[move.corner_permutation[i]] + move.corner_orientation[i]) % 3); }
                for (int i = 0; i < 12; i++) { new_ep[i] = edge_permutation[move.edge_permutation[i]]; }
                for (int i = 0; i < 12; i++) { new_eo[i] = ((edge_orientation[move.edge_permutation[i]] + move.edge_orientation[i]) % 2); }

                return new Cube(new_cp, new_co, new_ep, new_eo);
            }

            public void copy(Cube other)
            {
                for (int i = 0; i < 8; i++) { corner_permutation[i] = other.corner_permutation[i]; }
                for (int i = 0; i < 8; i++) { corner_orientation[i] = other.corner_orientation[i]; }
                for (int i = 0; i < 12; i++) { edge_permutation[i] = other.edge_permutation[i]; }
                for (int i = 0; i < 12; i++) { edge_orientation[i] = other.edge_orientation[i]; }
            }
        }

        public class Move
        {
            public Dictionary<string, int> move_names2int = new Dictionary<string, int>()
            {
                {"U",11},
                {"D",12},
                {"L",13},
                {"R",14},
                {"F",15},
                {"B",16},
                {"U2",21},
                {"D2",22},
                {"L2",23},
                {"R2",24},
                {"F2",25},
                {"B2",26},
                {"U`",31},
                {"D`",32},
                {"L`",33},
                {"R`",34},
                {"F`",35},
                {"B`",36}
            };

            public string[] move_names = { "U", "U2", "U`", "D", "D2", "D`", "L", "L2", "L`", "R", "R2", "R`", "F", "F2", "F`", "B", "B2", "B`" };

            public int[] U_cp = { 3, 0, 1, 2, 4, 5, 6, 7 };
            public int[] U_co = { 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] U_ep = { 0, 1, 2, 3, 7, 4, 5, 6, 8, 9, 10, 11 };
            public int[] U_eo = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] D_cp = { 0, 1, 2, 3, 5, 6, 7, 4 };
            public int[] D_co = { 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] D_ep = { 0, 1, 2, 3, 4, 5, 6, 7, 9, 10, 11, 8 };
            public int[] D_eo = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] L_cp = { 4, 1, 2, 0, 7, 5, 6, 3 };
            public int[] L_co = { 2, 0, 0, 1, 1, 0, 0, 2 };
            public int[] L_ep = { 11, 1, 2, 7, 4, 5, 6, 0, 8, 9, 10, 3 };
            public int[] L_eo = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] R_cp = { 0, 2, 6, 3, 4, 1, 5, 7 };
            public int[] R_co = { 0, 1, 2, 0, 0, 2, 1, 0 };
            public int[] R_ep = { 0, 5, 9, 3, 4, 2, 6, 7, 8, 1, 10, 11 };
            public int[] R_eo = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            public int[] F_cp = { 0, 1, 3, 7, 4, 5, 2, 6 };
            public int[] F_co = { 0, 0, 1, 2, 0, 0, 2, 1 };
            public int[] F_ep = { 0, 1, 6, 10, 4, 5, 3, 7, 8, 9, 2, 11 };
            public int[] F_eo = { 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0 };
            public int[] B_cp = { 1, 5, 2, 3, 0, 4, 6, 7 };
            public int[] B_co = { 1, 2, 0, 0, 2, 1, 0, 0 };
            public int[] B_ep = { 4, 8, 2, 3, 1, 5, 6, 7, 0, 9, 10, 11 };
            public int[] B_eo = { 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0 };

            public Cube moves(string move_name)
            {
                Cube U = new Cube(U_cp, U_co, U_ep, U_eo);
                Cube D = new Cube(D_cp, D_co, D_ep, D_eo);
                Cube L = new Cube(L_cp, L_co, L_ep, L_eo);
                Cube R = new Cube(R_cp, R_co, R_ep, R_eo);
                Cube F = new Cube(F_cp, F_co, F_ep, F_eo);
                Cube B = new Cube(B_cp, B_co, B_ep, B_eo);

                Cube dst = new Cube(U_cp, U_co, U_ep, U_eo);
                Cube tmp = new Cube(U_cp, U_co, U_ep, U_eo);

                switch (move_names2int[move_name] % 10)
                {
                    case 1: { dst.copy(U); tmp.copy(U); break; }
                    case 2: { dst.copy(D); tmp.copy(D); break; }
                    case 3: { dst.copy(L); tmp.copy(L); break; }
                    case 4: { dst.copy(R); tmp.copy(R); break; }
                    case 5: { dst.copy(F); tmp.copy(F); break; }
                    case 6: { dst.copy(B); tmp.copy(B); break; }
                }

                switch (move_names2int[move_name] / 10)
                {
                    case 1: break;
                    case 2: { dst.copy(dst.move_cube(tmp)); break; }
                    case 3: { dst.copy(dst.move_cube(tmp)); dst.copy(dst.move_cube(tmp)); break; }
                }

                return dst;
            }
        }
        
        public static class table
        {
            public static bool first_load = true; 

            public static string[] move_names_ph2 = { "U", "U2", "U`", "D", "D2", "D`", "L2", "R2", "F2", "B2" };

            public static int[,] co_move_table = new int[NUM_CO, MOVE_NAMES_LEN];
            public static int[,] eo_move_table = new int[NUM_EO, MOVE_NAMES_LEN];
            public static int[,] e_combination_table = new int[NUM_E_COMBINATIONS, MOVE_NAMES_LEN];

            public static int[,] cp_move_table = new int[NUM_CP, MOVE_NAMES_PH2_LEN];
            public static int[,] ud_ep_move_table = new int[NUM_UD_EP, MOVE_NAMES_PH2_LEN];
            public static int[,] e_ep_move_table = new int[NUM_E_EP, MOVE_NAMES_PH2_LEN];

            public static int[,] co_eec_prune_table = new int[NUM_CO, NUM_E_COMBINATIONS];
            public static int[,] eo_eec_prune_table = new int[NUM_EO, NUM_E_COMBINATIONS];

            public static int[,] cp_eep_prune_table = new int[NUM_CP, NUM_E_EP];
            public static int[,] udep_eep_prune_table = new int[NUM_UD_EP, NUM_E_EP];

            private static int[,] ReadCSV(string filePath,int rows,int cols)
            {
                TextAsset csvFile;
                csvFile = Resources.Load(filePath) as TextAsset; // Resources�ɂ���CSV�t�@�C�����i�[
                StringReader reader = new StringReader(csvFile.text); // TextAsset��StringReader�ɕϊ�

                if (csvFile == null)
                {
                    Debug.LogError("CSV file not found at path: " + filePath);
                    return null; // �G���[�����A�K�؂ɑΉ����Ă�������
                }
                if (reader == null)
                {
                    Debug.LogError("Failed to read CSV file.");
                    return null; // �G���[�����A�K�؂ɑΉ����Ă�������
                }

                int[,] array2D = new int[rows, cols];

                int i = 0;
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine(); // 1�s���ǂݍ���
                    string[] values = line.Split(','); // csvData���X�g�ɒǉ�����
                    for (int j = 0; j < cols; j++)
                    {
                        array2D[i, j] = int.Parse(values[j]);
                    }
                    i++;
                }
                return array2D;
            }

            public static int co_to_index(int[] co)
            {
                int index = 0;
                for (int i = 0; i < 7; i++)
                {
                    index *= 3;
                    index += co[i];
                }
                return index;
            }

            public static void index_to_co(int index, int[] co)
            {
                for (int i = 0; i < 8; i++) { co[i] = 0; }
                int sum_co = 0;
                for (int i = 6; i >= 0; i--)
                {
                    co[i] = index % 3;
                    index = (int)(index / 3);
                    sum_co += co[i];
                }
                co[7] = (3 - sum_co % 3) % 3;
            }

            public static int eo_to_index(int[] eo)
            {
                int index = 0;
                for (int i = 0; i < 11; i++)
                {
                    index *= 2;
                    index += eo[i];
                }
                return index;
            }

            public static int calc_combination(int n, int r)
            {
                int ret = 1;
                for (int i = 0; i < r; i++)
                {
                    ret *= n - i;
                }
                for (int i = 0; i < r; i++)
                {
                    ret = (int)(ret / (r - i));
                }
                return ret;
            }

            public static int e_combination_to_index(int[] comb)
            {
                int index = 0;
                int r = 4;
                for (int i = 11; i >= 0; i--)
                {
                    if (comb[i] != 0)
                    {
                        index += calc_combination(i, r);
                        r--;
                    }
                }
                return index;
            }

            public static int cp_to_index(int[] cp)
            {
                int index = 0;
                for (int i = 0; i < 8; i++)
                {
                    index *= 8 - i;
                    for (int j = i + 1; j < 8; j++)
                    {
                        if (cp[i] > cp[j])
                        {
                            index++;
                        }
                    }
                }
                return index;
            }

            public static int ud_ep_to_index(int[] ep)
            {
                int index = 0;
                for (int i = 0; i < 8; i++)
                {
                    index *= 8 - i;
                    for (int j = i + 1; j < 8; j++)
                    {
                        if (ep[i] > ep[j])
                        {
                            index++;
                        }
                    }
                }
                return index;
            }

            public static int e_ep_to_index(int[] eep)
            {
                int index = 0;
                for (int i = 0; i < 4; i++)
                {
                    index *= 4 - i;
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (eep[i] > eep[j])
                        {
                            index++;
                        }
                    }
                }
                return index;
            }

            public static void table_init()
            {
                co_move_table = ReadCSV("co_move_table",NUM_CO,MOVE_NAMES_LEN);
                eo_move_table = ReadCSV("eo_move_table",NUM_EO,MOVE_NAMES_LEN);
                e_combination_table = ReadCSV("e_combination_table",NUM_E_COMBINATIONS,MOVE_NAMES_LEN);
                cp_move_table = ReadCSV("cp_move_table",NUM_CP,MOVE_NAMES_PH2_LEN);
                ud_ep_move_table = ReadCSV("ud_ep_move_table",NUM_UD_EP,MOVE_NAMES_PH2_LEN);
                e_ep_move_table = ReadCSV("e_ep_move_table",NUM_E_EP,MOVE_NAMES_PH2_LEN);
                co_eec_prune_table = ReadCSV("co_eec_prune_table",NUM_CO,NUM_E_COMBINATIONS);
                eo_eec_prune_table = ReadCSV("eo_eec_prune_table",NUM_EO,NUM_E_COMBINATIONS);
                cp_eep_prune_table = ReadCSV("cp_eep_prune_table",NUM_CP,NUM_E_EP);
                udep_eep_prune_table = ReadCSV("udep_eep_prune_table",NUM_UD_EP,NUM_E_EP);
                first_load = false;
            }
        }

        public class Search
        {

            private Move move = new Move();

            private static int[] solved_cp = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
            private static int[] solved_co = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            private static int[] solved_ep = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            private static int[] solved_eo = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            private Cube initial_cube = new Cube(solved_cp, solved_co, solved_ep, solved_eo);

            private List<string> current_solution_ph1 = new List<string>();
            private List<string> current_solution_ph2 = new List<string>();

            private int max_solution_length = 9999;
            private System.Diagnostics.Stopwatch sw = null;
            private double max_time = 1;

            private Dictionary<int, int> inv_face = new Dictionary<int, int>()
            {
                {1,2},
                {2,1},
                {3,4},
                {4,3},
                {5,6},
                {6,5}
            };

            private Dictionary<int, int> sort_dict_face = new Dictionary<int, int>()
            {
                {1,6},
                {2,2},
                {3,4},
                {4,5},
                {5,3},
                {6,1}
            };

            public bool check_solved(Cube cube)
            {
                for (int i = 0; i < 12; i++) { if (cube.edge_orientation[i] != 0) return false; }
                for (int i = 0; i < 8; i++) { if (cube.corner_orientation[i] != 0) return false; }
                for (int i = 0; i < 12; i++) { if (cube.edge_permutation[i] != i) return false; }
                for (int i = 0; i < 8; i++) { if (cube.corner_permutation[i] != i) return false; }
                return true;
            }

            public bool check_move_available(string prev_move, string later_move)
            {
                var result = move.move_names2int.FirstOrDefault(item => item.Key == prev_move);
                if (result.Key == null)
                {
                    return true;
                }
                int prev = move.move_names2int[prev_move] % 10;
                int later = move.move_names2int[later_move] % 10;
                if (prev == later)
                {
                    return false;
                }
                if (inv_face[prev] == later)
                {
                    return sort_dict_face[prev] < sort_dict_face[later];
                }
                return true;
            }

            public void init(Cube cube)
            {
                initial_cube = cube;
                current_solution_ph1 = new List<string>();
                sw = new System.Diagnostics.Stopwatch();
            }

            public bool depth_limited_search_ph2(int cp_index, int udep_index, int eep_index, int depth)
            {
                if (depth == 0 && cp_index == 0 && udep_index == 0 && eep_index == 0)
                {
                    return true;
                }
                if (depth == 0)
                {
                    return false;
                }

                sw.Stop();
                if((double)(sw.ElapsedMilliseconds / 1000) > max_time)
                {
                    return false;
                }
                sw.Restart();

                int max_value;
                if (table.cp_eep_prune_table[cp_index, eep_index] > table.udep_eep_prune_table[udep_index, eep_index])
                {
                    max_value = table.cp_eep_prune_table[cp_index, eep_index];
                }
                else
                {
                    max_value = table.udep_eep_prune_table[udep_index, eep_index];
                }
                if (max_value > depth)
                {
                    return false;
                }

                string prev_move;
                if (current_solution_ph2.Count > 0)
                {
                    prev_move = current_solution_ph2[current_solution_ph2.Count - 1];
                }
                else if (current_solution_ph1.Count > 0)
                {
                    prev_move = current_solution_ph1[current_solution_ph1.Count - 1];
                }
                else
                {
                    prev_move = "NONE";
                }

                for (int i = 0; i < MOVE_NAMES_PH2_LEN; i++)
                {
                    if (check_move_available(prev_move, table.move_names_ph2[i]) != true)
                    {
                        continue;
                    }
                    current_solution_ph2.Add(table.move_names_ph2[i]);
                    int next_cp_index = table.cp_move_table[cp_index, i];
                    int next_udep_index = table.ud_ep_move_table[udep_index, i];
                    int next_eep_index = table.e_ep_move_table[eep_index, i];
                    if (depth_limited_search_ph2(next_cp_index, next_udep_index, next_eep_index, depth - 1))
                    {
                        return true;
                    }
                    current_solution_ph2.RemoveAt(current_solution_ph2.Count - 1);
                }

                return false;
            }

            public bool start_phase2(Cube cube)
            {
                int cp_index = table.cp_to_index(cube.corner_permutation);
                int[] tmp_ep1 = new int[8];
                int[] tmp_ep2 = new int[4];
                for (int i = 0; i < 8; i++) { tmp_ep1[i] = cube.edge_permutation[i + 4]; }
                for (int i = 0; i < 4; i++) { tmp_ep2[i] = cube.edge_permutation[i]; }
                int udep_index = table.ud_ep_to_index(tmp_ep1);
                int eep_index = table.e_ep_to_index(tmp_ep2);
                for (int depth = 0; depth <= max_solution_length - current_solution_ph1.Count; depth++)
                {
                    if (depth_limited_search_ph2(cp_index, udep_index, eep_index, depth))
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool depth_limited_search_ph1(int co_index, int eo_index, int e_comb_index, int depth)
            {
                if (depth == 0 && co_index == 0 && eo_index == 0 && e_comb_index == 0)
                {
                    Cube cube = new Cube(solved_cp, solved_co, solved_ep, solved_eo);
                    cube = initial_cube;
                    for (int i = 0; i < current_solution_ph1.Count; i++)
                    {
                        cube = cube.move_cube(move.moves(current_solution_ph1[i]));
                    }
                    return start_phase2(cube);
                }
                if (depth == 0)
                {
                    return false;
                }

                sw.Stop();
                if ((double)(sw.ElapsedMilliseconds / 1000) > max_time)
                {
                    return false;
                }
                sw.Restart();

                int max_value;
                if (table.co_eec_prune_table[co_index, e_comb_index] > table.eo_eec_prune_table[eo_index, e_comb_index])
                {
                    max_value = table.co_eec_prune_table[co_index, e_comb_index];
                }
                else
                {
                    max_value = table.eo_eec_prune_table[eo_index, e_comb_index];
                }
                if (max_value > depth)
                {
                    return false;
                }

                string prev_move;
                if (current_solution_ph1.Count > 0)
                {
                    prev_move = current_solution_ph1[current_solution_ph1.Count - 1];
                }
                else
                {
                    prev_move = "NONE";
                }

                for (int i = 0; i < MOVE_NAMES_LEN; i++)
                {
                    if (check_move_available(prev_move, move.move_names[i]) != true)
                    {
                        continue;
                    }
                    current_solution_ph1.Add(move.move_names[i]);
                    int next_co_index = table.co_move_table[co_index, i];
                    int next_eo_index = table.eo_move_table[eo_index, i];
                    int next_e_comb_index = table.e_combination_table[e_comb_index, i];
                    if (depth_limited_search_ph1(next_co_index, next_eo_index, next_e_comb_index, depth - 1))
                    {
                        return true;
                    }
                    current_solution_ph1.RemoveAt(current_solution_ph1.Count - 1);
                }

                return false;
            }

            public string start_search(int max_length = 30)
            {
                string result = "";

                max_solution_length = max_length;

                sw.Start();

                int co_index = table.co_to_index(initial_cube.corner_orientation);
                int eo_index = table.eo_to_index(initial_cube.edge_orientation);
                int[] e_combination = new int[12];
                for (int i = 0; i < 12; i++)
                {
                    int e = initial_cube.edge_permutation[i];
                    if (e == 0 || e == 1 || e == 2 || e == 3)
                    {
                        e_combination[i] = 1;
                    }
                    else
                    {
                        e_combination[i] = 0;
                    }
                }
                int e_comb_index = table.e_combination_to_index(e_combination);

                for (int depth = 0; depth <= max_length; depth++)
                {
                    if (depth_limited_search_ph1(co_index, eo_index, e_comb_index, depth))
                    {
                        for (int i = 0; i < current_solution_ph1.Count; i++)
                        {
                            result += current_solution_ph1[i] + " ";
                        }
                        for (int i = 0; i < current_solution_ph2.Count; i++)
                        {
                            result += current_solution_ph2[i] + " ";
                        }
                        return result;
                    }
                }
                return result;
            }

        }

        public class Translater
        {
            private enum COLOR
            {
                U, R, F, D, L, B
            }

            private enum FACELET
            {
                U1, U2, U3, U4, U5, U6, U7, U8, U9,
                R1, R2, R3, R4, R5, R6, R7, R8, R9,
                F1, F2, F3, F4, F5, F6, F7, F8, F9,
                D1, D2, D3, D4, D5, D6, D7, D8, D9,
                L1, L2, L3, L4, L5, L6, L7, L8, L9,
                B1, B2, B3, B4, B5, B6, B7, B8, B9
            }

            private int[,] corner_facelet = new int[8, 3]
            {
                { (int)FACELET.U1, (int)FACELET.L1, (int)FACELET.B3 },
                { (int)FACELET.U3, (int)FACELET.B1, (int)FACELET.R3 },
                { (int)FACELET.U9, (int)FACELET.R1, (int)FACELET.F3 },
                { (int)FACELET.U7, (int)FACELET.F1, (int)FACELET.L3 },
                { (int)FACELET.D7, (int)FACELET.B9, (int)FACELET.L7 },
                { (int)FACELET.D9, (int)FACELET.R9, (int)FACELET.B7 },
                { (int)FACELET.D3, (int)FACELET.F9, (int)FACELET.R7 },
                { (int)FACELET.D1, (int)FACELET.L9, (int)FACELET.F7 }
            };

            private int[,] edge_facelet = new int[12, 2]
            {
                { (int)FACELET.B6, (int)FACELET.L4 },
                { (int)FACELET.B4, (int)FACELET.R6 },
                { (int)FACELET.F6, (int)FACELET.R4 },
                { (int)FACELET.F4, (int)FACELET.L6 },
                { (int)FACELET.U2, (int)FACELET.B2 },
                { (int)FACELET.U6, (int)FACELET.R2 },
                { (int)FACELET.U8, (int)FACELET.F2 },
                { (int)FACELET.U4, (int)FACELET.L2 },
                { (int)FACELET.D8, (int)FACELET.B8 },
                { (int)FACELET.D6, (int)FACELET.R8 },
                { (int)FACELET.D2, (int)FACELET.F8 },
                { (int)FACELET.D4, (int)FACELET.L8 }
            };

            private int[,] corner_color = new int[8, 3]
            {
                { (int)COLOR.U, (int)COLOR.L, (int)COLOR.B },
                { (int)COLOR.U, (int)COLOR.B, (int)COLOR.R },
                { (int)COLOR.U, (int)COLOR.R, (int)COLOR.F },
                { (int)COLOR.U, (int)COLOR.F, (int)COLOR.L },
                { (int)COLOR.D, (int)COLOR.B, (int)COLOR.L },
                { (int)COLOR.D, (int)COLOR.R, (int)COLOR.B },
                { (int)COLOR.D, (int)COLOR.F, (int)COLOR.R },
                { (int)COLOR.D, (int)COLOR.L, (int)COLOR.F }
            };

            private int[,] edge_color = new int[12, 2]
            {
                { (int)COLOR.B, (int)COLOR.L },
                { (int)COLOR.B, (int)COLOR.R },
                { (int)COLOR.F, (int)COLOR.R },
                { (int)COLOR.F, (int)COLOR.L },
                { (int)COLOR.U, (int)COLOR.B },
                { (int)COLOR.U, (int)COLOR.R },
                { (int)COLOR.U, (int)COLOR.F },
                { (int)COLOR.U, (int)COLOR.L },
                { (int)COLOR.D, (int)COLOR.B },
                { (int)COLOR.D, (int)COLOR.R },
                { (int)COLOR.D, (int)COLOR.F },
                { (int)COLOR.D, (int)COLOR.L }
            };

            private enum CORNER
            {
                URF, UFL, ULB, UBR, DFR, DLF, DBL, DRB
            }

            private enum EDGE
            {
                UR, UF, UL, UB, DR, DF, DL, DB, FR, FL, BL, BR
            }

            private class FaceCube
            {
                public int[] f = new int[54];
            }

            private static int[] solved_cp = new int[8] { 0, 1, 2, 3, 4, 5, 6, 7 };
            private static int[] solved_co = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            private static int[] solved_ep = new int[12] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            private static int[] solved_eo = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            private void cubiecube_construct(Cube cubie)
            {
                // corner permutation
                int[] _cp = new int[8] { (int)CORNER.URF, (int)CORNER.UFL, (int)CORNER.ULB, (int)CORNER.UBR, (int)CORNER.DFR, (int)CORNER.DLF, (int)CORNER.DBL, (int)CORNER.DRB };
                // corner orientation
                int[] _co = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
                // edge permutation
                int[] _ep = new int[12] { (int)EDGE.UR, (int)EDGE.UF, (int)EDGE.UL, (int)EDGE.UB, (int)EDGE.DR, (int)EDGE.DF, (int)EDGE.DL, (int)EDGE.DB, (int)EDGE.FR, (int)EDGE.FL, (int)EDGE.BL, (int)EDGE.BR };
                // edge orientation
                int[] _eo = new int[12] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                cubie = new Cube(_cp, _co, _ep, _eo);
            }

            private void facecube_construct(FaceCube fc)
            {
                string str = "UUUUUUUUURRRRRRRRRFFFFFFFFFDDDDDDDDDLLLLLLLLLBBBBBBBBB";
                int i;

                for (i = 0; i < 54; i++)
                {
                    switch (str[i])
                    {
                        case 'U':
                            fc.f[i] = 0; break;
                        case 'R':
                            fc.f[i] = 1; break;
                        case 'F':
                            fc.f[i] = 2; break;
                        case 'D':
                            fc.f[i] = 3; break;
                        case 'L':
                            fc.f[i] = 4; break;
                        case 'B':
                            fc.f[i] = 5; break;
                    }
                }
            }

            // Construct a facelet cube from a string
            // �����񂩂�facecube�𓾂�
            private void facecube_construct_str(FaceCube fc, string cube_str)
            {
                for (int i = 0; i < cube_str.Length; i++)
                {
                    switch (cube_str[i])
                    {
                        case 'U':
                            fc.f[i] = 0; break;
                        case 'R':
                            fc.f[i] = 1; break;
                        case 'F':
                            fc.f[i] = 2; break;
                        case 'D':
                            fc.f[i] = 3; break;
                        case 'L':
                            fc.f[i] = 4; break;
                        case 'B':
                            fc.f[i] = 5; break;
                    }
                }
            }

            // Gives string representation of a facelet cube
            // facecube���當����𓾂�
            private void to_string(FaceCube fc, string s)
            {
                s = "";
                string[] to = new string[6] { "U", "R", "F", "D", "L", "B" };
                for (int i = 0; i < 54; i++)
                {
                    s += to[fc.f[i]];
                }
            }

            // Gives CubieCube representation of a faceletcube
            // facecube����cubiecube�𓾂�
            private bool to_cubiecube(FaceCube fc, Cube ccret)
            {
                int col1, col2;
                int ori, i, j;
                int corner_counter = 0;
                int edge_counter = 0;

                cubiecube_construct(ccret);
                for (i = 0; i < 8; i++)
                {
                    ccret.corner_permutation[i] = (int)CORNER.URF; // invalidate corners
                }
                for (i = 0; i < 12; i++)
                {
                    ccret.edge_permutation[i] = (int)EDGE.UR; // and edges
                }
                for (i = 0; i < 8; i++)
                {
                    // get the colors of the cubie at corner i, starting with U/D
                    for (ori = 0; ori < 3; ori++)
                    {
                        if (fc.f[(int)corner_facelet[i, ori]] == (int)COLOR.U || fc.f[(int)corner_facelet[i, ori]] == (int)COLOR.D)
                        {
                            break;
                        }
                    }
                    col1 = fc.f[(int)corner_facelet[i, (ori + 1) % 3]];
                    col2 = fc.f[(int)corner_facelet[i, (ori + 2) % 3]];
                    for (j = 0; j < 8; j++)
                    {
                        if (col1 == corner_color[j, 1] && col2 == corner_color[j, 2])
                        {
                            // in cornerposition i we have cornercubie j
                            ccret.corner_permutation[i] = j;
                            ccret.corner_orientation[i] = ori % 3;
                            corner_counter++;
                            break;
                        }
                        
                    }
                }
                for (i = 0; i < 12; i++)
                {
                    for (j = 0; j < 12; j++)
                    {
                        if (fc.f[(int)edge_facelet[i, 0]] == edge_color[j, 0] && fc.f[(int)edge_facelet[i, 1]] == edge_color[j, 1])
                        {
                            ccret.edge_permutation[i] = j;
                            ccret.edge_orientation[i] = 0;
                            edge_counter++;
                            break;
                        }
                        if (fc.f[(int)edge_facelet[i, 0]] == edge_color[j, 1] && fc.f[(int)edge_facelet[i, 1]] == edge_color[j, 0])
                        {
                            ccret.edge_permutation[i] = j;
                            ccret.edge_orientation[i] = 1;
                            edge_counter++;
                            break;
                        }
                        
                    }
                }

                if(corner_counter == 8 && edge_counter == 12)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }

            // return cube in facelet representation
            private void to_facecube(Cube cubie, FaceCube fcret)
            {
                int i, j, n;
                int ori;

                facecube_construct(fcret);
                for (i = 0; i < 8; i++)
                {
                    j = cubie.corner_permutation[i]; // cornercubie with index j is at
                                                     // cornerposition with index i
                    ori = cubie.corner_orientation[i]; // Orientation of this cubie
                    for (n = 0; n < 3; n++)
                    {
                        fcret.f[corner_facelet[i, (n + ori) % 3]] = corner_color[j, n];
                    }
                }
                for (i = 0; i < 12; i++)
                {
                    j = cubie.edge_permutation[i]; // edgecubie with index j is at edgeposition
                                                   // with index i
                    ori = cubie.edge_orientation[i]; // Orientation of this cubie
                    for (n = 0; n < 2; n++)
                    {
                        fcret.f[edge_facelet[i, (n + ori) % 2]] = edge_color[j, n];
                    }
                }
            }

            public Cube face2Cube(string face)
            {
                FaceCube fc = new FaceCube();
                Cube ccret = new Cube(solved_cp, solved_co, solved_ep, solved_eo);

                string cube_str = face;
                facecube_construct(fc);
                facecube_construct_str(fc, cube_str);

                if (to_cubiecube(fc, ccret))
                {
                    return ccret;
                }
                else
                {
                    return null;
                }
                
            }

            public string Cube2face(Cube cube)
            {
                Cube ccret = new Cube(solved_cp, solved_co, solved_ep, solved_eo);
                cubiecube_construct(ccret);

                FaceCube fc = new FaceCube();

                string result = "";

                ccret.copy(cube);

                to_facecube(ccret, fc);
                to_string(fc, result);

                return result;
            }
        }

        Vector3[] rubic_pos = new Vector3[54]
        {
            //UP
            new Vector3(-1,-1,1.483f),new Vector3(0,-1,1.483f),new Vector3(1,-1,1.483f),
            new Vector3(-1,0,1.483f),new Vector3(0,0,1.483f),new Vector3(1,0,1.483f),
            new Vector3(-1,1,1.483f),new Vector3(0,1,1.483f),new Vector3(1,1,1.483f),
            //RIGHT
            new Vector3(1.483f,1,1),new Vector3(1.483f,0,1),new Vector3(1.483f,-1,1),
            new Vector3(1.483f,1,0),new Vector3(1.483f,0,0),new Vector3(1.483f,-1,0),
            new Vector3(1.483f,1,-1),new Vector3(1.483f,0,-1),new Vector3(1.483f,-1,-1),
            //FRONT
            new Vector3(-1,1.483f,1),new Vector3(0,1.483f,1),new Vector3(1,1.483f,1),
            new Vector3(-1,1.483f,0),new Vector3(0,1.483f,0),new Vector3(1,1.483f,0),
            new Vector3(-1,1.483f,-1),new Vector3(0,1.483f,-1),new Vector3(1,1.483f,-1),
            //DOWN
            new Vector3(-1,1,-1.483f),new Vector3(0,1,-1.483f),new Vector3(1,1,-1.483f),
            new Vector3(-1,0,-1.483f),new Vector3(0,0,-1.483f),new Vector3(1,0,-1.483f),
            new Vector3(-1,-1,-1.483f),new Vector3(0,-1,-1.483f),new Vector3(1,-1,-1.483f),
            //LEFT
            new Vector3(-1.483f,-1,1),new Vector3(-1.483f,0,1),new Vector3(-1.483f,1,1),
            new Vector3(-1.483f,-1,0),new Vector3(-1.483f,0,0),new Vector3(-1.483f,1,0),
            new Vector3(-1.483f,-1,-1),new Vector3(-1.483f,0,-1),new Vector3(-1.483f,1,-1),
            //BACK
            new Vector3(1,-1.483f,1),new Vector3(0,-1.483f,1),new Vector3(-1,-1.483f,1),
            new Vector3(1,-1.483f,0),new Vector3(0,-1.483f,0),new Vector3(-1,-1.483f,0),
            new Vector3(1,-1.483f,-1),new Vector3(0,-1.483f,-1),new Vector3(-1,-1.483f,-1),
        };

        string[] rubic_plane = new string[54];

        [SerializeField] Move_data move_data;
        [SerializeField] Plane_Colors plane_colors;

        Search search = null;
        Translater translater = null;

        // Start is called before the first frame update
        void Start()
        {
            move_data.moves = "";

            search = new Search();
            translater = new Translater();
            if(table.first_load)
            {
                table.table_init();
            }
            for (int i = 0; i < 54;i++)    
            {
                rubic_plane[i] = "Plane" + i;
            }
        }

        string check_color()
        {
            string result = "";
            for (int i = 0; i < 54; i++)
            {
                Vector3 tmp_pos = rubic_pos[i];
                for (int j = 0; j < 54; j++)
                {
                    GameObject tmp_object = GameObject.Find(rubic_plane[j]);
                    if (tmp_pos == tmp_object.transform.position)
                    {
                        if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 1, 1, 1)) { result += "U"; }
                        else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 0, 0, 1)) { result += "R"; }
                        else if (tmp_object.GetComponent<Renderer>().material.color == new Color(0, 1, 0, 1)) { result += "F"; }
                        else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 1, 0, 1)) { result += "D"; }
                        else if (tmp_object.GetComponent<Renderer>().material.color == new Color(1, 0.5f, 0, 1)) { result += "L"; }
                        else if (tmp_object.GetComponent<Renderer>().material.color == new Color(0, 0, 1, 1)) { result += "B"; }
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return result;
        }

        public void Onclick()
        {
            string rubic_color_code = check_color();
            plane_colors.planes = rubic_color_code;
            Debug.Log(rubic_color_code);
            if (rubic_color_code.Length == 54)
            {
                Cube cube = translater.face2Cube(rubic_color_code);
                if(cube != null)
                {
                    search.init(cube);
                    move_data.moves = search.start_search();
                    Debug.Log(move_data.moves);
                }
                else
                {
                    move_data.moves = "";
                }
            }
            if(move_data.moves.Split(' ').Length == 1)
            {
                GameObject error_tmp = GameObject.Find("error_message(Clone)");
                if(error_tmp == null)
                {
                    Debug.Log("Can not solve the Rubic Cube");
                    GameObject obj = (GameObject)Resources.Load ("error_message");
                    GameObject parent = GameObject.Find("Canvas");
                    Instantiate(obj,parent.transform.position, Quaternion.identity, parent.transform);
                }
            }
            else
            {
                SceneManager.LoadScene("scene2");
            }
            
        }

    }
}


