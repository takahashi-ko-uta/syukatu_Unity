//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;


//public class Astar : MonoBehaviour
//{
//    private const int MapWidth = 11;
//    private const int MapHeight = 11;
//    private const int Infinity = 100000;

//    // コストテーブル
//    int[,] CostTable = new int[MapHeight, MapWidth];
//    // グラフ
//    Node[,] Map = new Node[MapHeight, MapWidth];
//    //ルートの保存用
//    Vector2Int[] routeSave = new Vector2Int[40];

//    // 削除結果
//    enum EraseResult
//    {
//        NotFound,       // 未発見
//        Erased,         // 削除
//        CouldntErased   // 削除できない
//    };

//    // ノード
//    private struct Node
//    {
//        public Vector2Int Position;         // ノード座標
//        public List<Node> AdjacentNodes;    // 隣接ノード(辺)
//        public float HeuristicCost;         // ヒューリスティックコスト
//        public float TotalCost;             // コスト(ヒューリスティックコスト込み)
//    };

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    //Node構造体の初期化
//    Node NodeInitialize(Node node_)
//    {
//        node_.Position = new Vector2Int(0, 0);
//        node_.HeuristicCost = Infinity;
//        node_.TotalCost = 0;
//        node_.AdjacentNodes.Clear();

//        return node_;
//    }

//    // 昇順ソート用関数
//    bool Less(Node a, Node b)
//    {
//        if (a.TotalCost < b.TotalCost)
//        {
//            return true;
//        }

//        return false;
//    }

//    // セル範囲チェック関数
//    bool IsCellWithinTheRange(int x, int y)
//    {
//        if (x >= 0 &&
//            x < MapWidth &&
//            y >= 0 &&
//            y < MapHeight)
//        {
//            return true;
//        }

//        return false;
//    }

//    // ノードの作成
//    void CreateMap(int[,] CostTable_)
//    {
//        for (int y = 0; y < MapHeight; y++)
//        {
//            for (int x = 0; x < MapWidth; x++)
//            {
//                this.CostTable[y, x] = CostTable_[y, x];
//                Map[y, x].Position.x = x;
//                Map[y, x].Position.y = y;

//                Vector2Int[] adjacent_cells = new Vector2Int[]
//                {
//                    new Vector2Int(x, y - 1),
//                    new Vector2Int(x - 1, y),
//                    new Vector2Int(x + 1, y),
//                    new Vector2Int(x, y + 1),
//                };

//                // 隣接ノードの追加
//                foreach (Vector2Int cell in adjacent_cells)
//                {
//                    if (IsCellWithinTheRange(cell.x, cell.y) == true &&
//                    CostTable_[cell.y, cell.x] == 1)
//                    {
//                        Map[y, x].AdjacentNodes.Add(Map[cell.y, cell.x]);
//                    }
//                }
//            }
//        }
//    }

//    // コスト初期化
//    void InitCost(int heuristic_cost, int total_cost)
//    {
//        for (int y = 0; y < MapHeight; y++)
//        {
//            for (int x = 0; x < MapWidth; x++)
//            {
//                Map[y, x].HeuristicCost = heuristic_cost;
//                Map[y, x].TotalCost = total_cost;
//            }
//        }
//    }

//    // ヒューリスティックコスト計算(ノードとゴールまでの距離を返している)
//    float CalculateHeuristic(Node node, Node Goal)
//    {

//        float x = Math.Abs(Goal.Position.x - node.Position.x);
//        float y = Math.Abs(Goal.Position.y - node.Position.y);

//        return MathF.Sqrt(x * x + y * y);
//    }

//    // セル比較
//    bool IsEqualCell(Vector2Int a, Vector2Int b)
//    {
//        if (a.x == b.x && a.y == b.y)
//        {
//            return true;
//        }

//        return false;
//    }

//    EraseResult EraseNode(LinkedList<Node> list, Node new_node, float new_cost)
//    {

//        // クローズリストチェック
//        foreach (var itr in list.GetEnumerator())
//        {
//            // ノードと同じセルがあるか調べる
//            if (IsEqualCell(new_node.Position, (*itr)->Position) == true)
//            {
//                // コストの比較
//                if (new_cost < (*itr)->TotalCost)
//                {
//                    list.erase(itr);
//                    return EraseResult.Erased;
//                }
//                else
//                {
//                    return EraseResult.CouldntErased;
//                }
//            }
//        }

//        return EraseResult.NotFound;
//    }

//    // オープンリストに追加
//    bool AddAdjacentNode(LinkedList<Node> open_list, LinkedList<Node> close_list, Node adjacent_node, float cost)
//    {
//        bool can_update = true;

//        LinkedList<Node>[] node_list =
//        {
//            close_list,
//            open_list
//        };

//        foreach (LinkedList<Node> list in node_list)
//        {
//            // リストに同じノードがあってリストの方のコストが高いなら削除
//            if (EraseNode(list, adjacent_node, cost) == EraseResult.CouldntErased)
//            {
//                can_update = false;
//            }
//        }

//        if (can_update == true)
//        {
//            open_list.AddLast(adjacent_node);
//            return true;
//        }

//        return false;
//    }

//    void AStar(Vector2Int start, Vector2Int goal)
//    {
//        LinkedList<Node> open_list = new LinkedList<Node>();
//        LinkedList<Node> close_list = new LinkedList<Node>();

//        //const Node* start_node = &Map[start.Y][start.X];
//        Node goal_node = Map[goal.y, goal.x];

//        // 更新したノード位置保存用
//        Vector2Int[,] last_update_cells = new Vector2Int[MapHeight, MapWidth];

//        // グラフの初期化
//        InitCost(Infinity, 0);

//        // スタートノードの指定
//        open_list.AddLast(Map[start.y, start.x]);

//        // 経路探索回数
//        int count = 0;

//        // オープンリストがなくなるまで回す
//        while (open_list.Count != 0)
//        {
//            count++;

//            Node search_node = open_list.GetEnumerator();
//            // 探索リストから除外
//            open_list.erase(open_list.GetEnumerator());

//            // ゴールに到達したら終わり
//            if (IsEqualCell(search_node.Position, goal) == true)
//            {
//                // クローズリストに最後のノードを追加する
//                close_list.AddLast(search_node);
//                break;
//            }

//            foreach (Node adjacent_node in search_node.AdjacentNodes)
//            {
//                // 計算を行っていなかった場合だけ計算
//                if (adjacent_node.HeuristicCost == Infinity)
//                {
//                    // ヒューリスティクスコスト計算
//                    adjacent_node.HeuristicCost = CalculateHeuristic(adjacent_node, goal_node);
//                }

//                // ノード間コスト
//                float edge_cost = CostTable[adjacent_node.Position.y, adjacent_node.Position.y];
//                // 取得ノードのトータルコスト
//                float node_cost = search_node.TotalCost;
//                /*
//                    トータルコスト算出
//                        ノード間コスト + ヒューリスティックコスト + 取得ノードのトータルコスト
//                */
//                float total_cost = edge_cost + adjacent_node.HeuristicCost + node_cost;

//                // ノード追加
//                if (AddAdjacentNode(open_list, close_list, adjacent_node, total_cost) == true)
//                {
//                    // トータルコストを更新
//                    adjacent_node.TotalCost = total_cost;

//                    if (adjacent_node.Position.y == 0 && adjacent_node.Position.x == 2)
//                    {
//                        int xx = 0;
//                        xx = 100;
//                    }

//                    // 経路を更新したセルを保存
//                    last_update_cells[adjacent_node.Position.y, adjacent_node.Position.x] = search_node.Position;
//                }
//            }

//            bool is_add_close = true;

//            // クローズリストチェック
//            for (var itr = close_list.GetEnumerator(); itr != close_list.end(); itr++;)
//            {
//                // ノードと同じセルがあるか調べる
//                if (IsEqualCell(search_node.Position, (*itr)->Position) == true)
//                {
//                    is_add_close = false;
//                    break;
//                }
//            }

//            // 同じノードが無かったので追加
//            if (is_add_close == true)
//            {
//                // このノードの探索終了
//                close_list.AddLast(search_node);
//            }
//        }

//        // 経路復元
//        LinkedList<Vector2Int> route_list = new LinkedList<Vector2Int>();

//        // ゴールセルから復元する
//        route_list.AddLast(goal);
//        while (route_list.Count != 0)
//        {
//            Vector2Int route = route_list.First.Value;
//            int num = 0;
//            // スタートセルなら終了
//            if (IsEqualCell(route, start) == true)
//            {
//                // 復元した経路を表示
//                foreach (Vector2Int vec2Int in route_list)
//                {
//                    routeSave[num].x = vec2Int.x;
//                    routeSave[num].y = vec2Int.y;
//                    num++;
//                }
//                for (int i = 0; i < 40; i++)
//                {
//                    if (i > num)
//                    {
//                        routeSave[num].x = 99;//99,99が出たら終わり
//                        routeSave[num].y = 99;
//                    }
//                }

//                break;
//            }
//            else
//            {
//                if (IsCellWithinTheRange(route.x, route.y) == true)
//                {
//                    // 追加
//                    route_list.AddFirst(last_update_cells[route.y, route.x]);
//                }
//                else
//                {
//                    //ルートなし
//                    break;
//                }
//            }

//        }
//    }
//}




