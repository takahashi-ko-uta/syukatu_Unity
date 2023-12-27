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

//    // �R�X�g�e�[�u��
//    int[,] CostTable = new int[MapHeight, MapWidth];
//    // �O���t
//    Node[,] Map = new Node[MapHeight, MapWidth];
//    //���[�g�̕ۑ��p
//    Vector2Int[] routeSave = new Vector2Int[40];

//    // �폜����
//    enum EraseResult
//    {
//        NotFound,       // ������
//        Erased,         // �폜
//        CouldntErased   // �폜�ł��Ȃ�
//    };

//    // �m�[�h
//    private struct Node
//    {
//        public Vector2Int Position;         // �m�[�h���W
//        public List<Node> AdjacentNodes;    // �אڃm�[�h(��)
//        public float HeuristicCost;         // �q���[���X�e�B�b�N�R�X�g
//        public float TotalCost;             // �R�X�g(�q���[���X�e�B�b�N�R�X�g����)
//    };

//    // Start is called before the first frame update
//    void Start()
//    {

//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    //Node�\���̂̏�����
//    Node NodeInitialize(Node node_)
//    {
//        node_.Position = new Vector2Int(0, 0);
//        node_.HeuristicCost = Infinity;
//        node_.TotalCost = 0;
//        node_.AdjacentNodes.Clear();

//        return node_;
//    }

//    // �����\�[�g�p�֐�
//    bool Less(Node a, Node b)
//    {
//        if (a.TotalCost < b.TotalCost)
//        {
//            return true;
//        }

//        return false;
//    }

//    // �Z���͈̓`�F�b�N�֐�
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

//    // �m�[�h�̍쐬
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

//                // �אڃm�[�h�̒ǉ�
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

//    // �R�X�g������
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

//    // �q���[���X�e�B�b�N�R�X�g�v�Z(�m�[�h�ƃS�[���܂ł̋�����Ԃ��Ă���)
//    float CalculateHeuristic(Node node, Node Goal)
//    {

//        float x = Math.Abs(Goal.Position.x - node.Position.x);
//        float y = Math.Abs(Goal.Position.y - node.Position.y);

//        return MathF.Sqrt(x * x + y * y);
//    }

//    // �Z����r
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

//        // �N���[�Y���X�g�`�F�b�N
//        foreach (var itr in list.GetEnumerator())
//        {
//            // �m�[�h�Ɠ����Z�������邩���ׂ�
//            if (IsEqualCell(new_node.Position, (*itr)->Position) == true)
//            {
//                // �R�X�g�̔�r
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

//    // �I�[�v�����X�g�ɒǉ�
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
//            // ���X�g�ɓ����m�[�h�������ă��X�g�̕��̃R�X�g�������Ȃ�폜
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

//        // �X�V�����m�[�h�ʒu�ۑ��p
//        Vector2Int[,] last_update_cells = new Vector2Int[MapHeight, MapWidth];

//        // �O���t�̏�����
//        InitCost(Infinity, 0);

//        // �X�^�[�g�m�[�h�̎w��
//        open_list.AddLast(Map[start.y, start.x]);

//        // �o�H�T����
//        int count = 0;

//        // �I�[�v�����X�g���Ȃ��Ȃ�܂ŉ�
//        while (open_list.Count != 0)
//        {
//            count++;

//            Node search_node = open_list.GetEnumerator();
//            // �T�����X�g���珜�O
//            open_list.erase(open_list.GetEnumerator());

//            // �S�[���ɓ��B������I���
//            if (IsEqualCell(search_node.Position, goal) == true)
//            {
//                // �N���[�Y���X�g�ɍŌ�̃m�[�h��ǉ�����
//                close_list.AddLast(search_node);
//                break;
//            }

//            foreach (Node adjacent_node in search_node.AdjacentNodes)
//            {
//                // �v�Z���s���Ă��Ȃ������ꍇ�����v�Z
//                if (adjacent_node.HeuristicCost == Infinity)
//                {
//                    // �q���[���X�e�B�N�X�R�X�g�v�Z
//                    adjacent_node.HeuristicCost = CalculateHeuristic(adjacent_node, goal_node);
//                }

//                // �m�[�h�ԃR�X�g
//                float edge_cost = CostTable[adjacent_node.Position.y, adjacent_node.Position.y];
//                // �擾�m�[�h�̃g�[�^���R�X�g
//                float node_cost = search_node.TotalCost;
//                /*
//                    �g�[�^���R�X�g�Z�o
//                        �m�[�h�ԃR�X�g + �q���[���X�e�B�b�N�R�X�g + �擾�m�[�h�̃g�[�^���R�X�g
//                */
//                float total_cost = edge_cost + adjacent_node.HeuristicCost + node_cost;

//                // �m�[�h�ǉ�
//                if (AddAdjacentNode(open_list, close_list, adjacent_node, total_cost) == true)
//                {
//                    // �g�[�^���R�X�g���X�V
//                    adjacent_node.TotalCost = total_cost;

//                    if (adjacent_node.Position.y == 0 && adjacent_node.Position.x == 2)
//                    {
//                        int xx = 0;
//                        xx = 100;
//                    }

//                    // �o�H���X�V�����Z����ۑ�
//                    last_update_cells[adjacent_node.Position.y, adjacent_node.Position.x] = search_node.Position;
//                }
//            }

//            bool is_add_close = true;

//            // �N���[�Y���X�g�`�F�b�N
//            for (var itr = close_list.GetEnumerator(); itr != close_list.end(); itr++;)
//            {
//                // �m�[�h�Ɠ����Z�������邩���ׂ�
//                if (IsEqualCell(search_node.Position, (*itr)->Position) == true)
//                {
//                    is_add_close = false;
//                    break;
//                }
//            }

//            // �����m�[�h�����������̂Œǉ�
//            if (is_add_close == true)
//            {
//                // ���̃m�[�h�̒T���I��
//                close_list.AddLast(search_node);
//            }
//        }

//        // �o�H����
//        LinkedList<Vector2Int> route_list = new LinkedList<Vector2Int>();

//        // �S�[���Z�����畜������
//        route_list.AddLast(goal);
//        while (route_list.Count != 0)
//        {
//            Vector2Int route = route_list.First.Value;
//            int num = 0;
//            // �X�^�[�g�Z���Ȃ�I��
//            if (IsEqualCell(route, start) == true)
//            {
//                // ���������o�H��\��
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
//                        routeSave[num].x = 99;//99,99���o����I���
//                        routeSave[num].y = 99;
//                    }
//                }

//                break;
//            }
//            else
//            {
//                if (IsCellWithinTheRange(route.x, route.y) == true)
//                {
//                    // �ǉ�
//                    route_list.AddFirst(last_update_cells[route.y, route.x]);
//                }
//                else
//                {
//                    //���[�g�Ȃ�
//                    break;
//                }
//            }

//        }
//    }
//}




