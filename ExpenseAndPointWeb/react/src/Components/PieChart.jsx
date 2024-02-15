import { React, useState, useEffect } from 'react';
import { Chart } from "react-google-charts";
import useWindowDimensions from '../Hooks/useWindowDimensions.jsx'


const PieChart = ({ expensesList, categoryList }) => {
    if (categoryList.length === 0) return <img src="/loading.gif"></img>;
    const { height, width } = useWindowDimensions();

    const [chartExpenses, setChartExpenses] = useState([
        ["Категория", "Сумма"],
        ["Нет расходов", 1]
    ]);

    const data = [
        ["Task", "Hours per Day"],
        ["Work", 11],
        ["Eat", 2],
        ["Commute", 2],
        ["Watch TV", 2],
        ["Sleep", 7],
    ];

    const[temp, setTemp] = useState([])
    useEffect(() => {
        var newArray = []
        newArray.push(["Категория", "Сумма"]);
        if (expensesList !== undefined && expensesList[0].id !== 0) {
            expensesList.map((e, index) => {
                if (categoryList !== undefined) {
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
        }
        setTemp(newArray);
    }, [expensesList]);

    const options = {
        title: "Диаграмма расходов",
        backgroundColor: {
            fill: 'white',
            fillOpacity: 0.1
        },
        legendTextStyle: { color: '#FFF' },
        titleTextStyle: { color: '#FFF' },
        width: 0.48 * width,
        height: 0.78 * height,
    };


    return (
        <>
            <Chart
                chartType="PieChart"
                data={temp}
                options={options}
            />
        </>
    );
}

export default PieChart;