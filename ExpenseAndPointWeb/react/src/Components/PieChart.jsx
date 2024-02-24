import { React, useState, useEffect } from 'react';
import { Chart } from "react-google-charts";
import useWindowDimensions from '../Hooks/useWindowDimensions.jsx'


const PieChart = ({ expensesList, categoryList }) => {
    const { height, width } = useWindowDimensions();

    const [chartExpenses, setChartExpenses] = useState([
    ]);

    const emptyExpensesList = [
        ["Категория", "Сумма"],
        ["Нет расходов", 1]
    ];

    useEffect(() => {
        if (expensesList !== null && expensesList.count !== 0) {
            var newArray = []
            newArray.push(["Категория", "Сумма"]);
            expensesList.map((e, index) => {
                if (categoryList !== undefined && categoryList.length !== 0) {
                    var elem = [categoryList.find(c => c.id === e.categoryId).title, e.amount];
                    var index = newArray.findIndex(t => t[0] === elem[0]);
                    if (index !== -1) {
                        var elemArray = newArray[index];
                        elemArray[1] = elemArray[1] + elem[1];
                    } else {
                        newArray.push(elem);
                    }
                }
            })
            setChartExpenses(newArray);
        }
    }, [expensesList]);

    if (categoryList.length === 0) return <img src="/loading.gif" style={{
        width: "100px",
        }}></img >;


    const options = {
        title: "Диаграмма расходов",
        backgroundColor: {
            fill: 'white',
            fillOpacity: 0.1
        },
        legendTextStyle: { color: '#FFF' },
        titleTextStyle: { color: '#FFF' },
        width: 0.48 * width,
        height: 0.4 * height,
    };

    return (
        <>
            <Chart
                chartType="PieChart"
                data={chartExpenses.length > 1 ? chartExpenses : emptyExpensesList}
                options={options}
            />
        </>
    );
}

export default PieChart;