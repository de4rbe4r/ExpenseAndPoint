import { React, useState, useEffect } from 'react';
import Cookies from 'universal-cookie';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { GetCategoryByIdUrl, AddExpensesUrl } from "../Urls/UrlList";
import Button from 'react-bootstrap/Button';


const AddExpenseBtn = ({ expenseToEdit, showModal }) => {
    const cookies = new Cookies();
    const [showExpense, setShowExpense] = useState(false);
    const handleCloseExpense = () => setShowExpense(false);

    const handleShowExpense = () => {
        setShowExpense(true);
        setExpense({...expense, categoryId: categoryList[0].id});
    }
    var today = new Date();
    var day = today.getDate();
    var month = today.getMonth() + 1;
    if (month < 10) {
        month = '0' + month;
    }
    var year = today.getFullYear();
    var currentDate = year + '-' + month + '-' + day;
    var hours = today.getHours();
    if (hours < 10) {
        hours = '0' + hours;
    }
    var minutes = today.getMinutes();
    if (minutes < 10) {
        minutes = '0' + minutes;
    }

    const [dateAndTime, setDateAndTime] = useState({
        date: currentDate,
        constPart: 'T',
        time: hours + ":" + minutes
    })

    const [categoryList, setCategoryList] = useState([]);
    useEffect(() => {
        const fetchData = async () => {
            const result = await axios.get(GetCategoryByIdUrl + cookies.get('userId')).catch(function (error) {
                if (error.response) {
                    alert(error.message + '\n' + error.response.data.detail);
                }
            });
            setCategoryList(result.data);
        };
        fetchData();
    }, []);

    const [expense, setExpense] = useState({
        amount: (expenseToEdit !== null) ? exepenseToEdit.amount : '',
        userId: (expenseToEdit !== null) ? exepenseToEdit.userId: cookies.get('userId'),
        dateTime: (expenseToEdit !== null) ? exepenseToEdit.dateTime : dateAndTime.date.toString() + dateAndTime.constPart + dateAndTime.time.toString(),
        categoryId: (expenseToEdit !== null) ? exepenseToEdit.categoryId : ''
    });

    const addExpense = (event) => {
        event.preventDefault();
        if (expense.amount === "") {
            alert("Введите сумму");
            return;
        }

        if (dateAndTime.date === "" || dateAndTime.time === "") {
            alert("Укажите дату и время");
            return;
        }
        axios.post(AddExpensesUrl, expense)
            .then(res => {
                alert("Расход успешно добавлен");
                setExpense({
                    ...expense,
                    amount: '',
                });
                setDateAndTime({
                    time: '',
                    date: '',
                    constPart: 'T'
                })
            })
            .catch(function (error) {
                if (error.response) {
                    alert(error.response.data.detail);
                }
            });
    }

    return (
        <>
            <Button  onClick={handleShowExpense}>asdasd</Button>
            <Modal
                show={showExpense}
                onHide={handleCloseExpense}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>Добавление расхода</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Label htmlFor="inputCategoryId">Категория</Form.Label>
                    <Form.Select
                        id="categoryId" onChange={event => {
                            setExpense({ ...expense, categoryId: event.target.value})
                        }}>
                        {categoryList.map((c, index) => (
                            <option value={c.id} key={c.id}>{c.title}</option>
                        ))}
                    </Form.Select>
                    <Form.Label htmlFor="inputDate">Дата</Form.Label>
                    <Form.Control
                        type="date"
                        id="inputDate"
                        defaultValue={dateAndTime.date}
                        onChange={event => {
                            setDateAndTime({ ...dateAndTime, date: event.target.value });
                            setExpense({ ...expense, dateTime: dateAndTime.date.toString() + dateAndTime.constPart + dateAndTime.time.toString() });
                        }
                        }
                    />
                    <Form.Label htmlFor="inputTime">Time</Form.Label>
                    <Form.Control
                        type="time"
                        id="inputTime"
                        defaultValue={dateAndTime.time}
                        onChange={event => {
                            setDateAndTime({ ...dateAndTime, time: event.target.value });
                            setExpense({ ...expense, dateTime: dateAndTime.date.toString() + dateAndTime.constPart + dateAndTime.time.toString() });
                        }
                        }
                    />
                    <Form.Label htmlFor="inputAmount">Сумма</Form.Label>
                    <Form.Control
                        type="number"
                        id="inputAmount"
                        step="0.01"
                        value={expense.amount} onChange={event => setExpense({ ...expense, amount: event.target.value })}
                    />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={addExpense}>Добавить</Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default AddExpenseBtn;