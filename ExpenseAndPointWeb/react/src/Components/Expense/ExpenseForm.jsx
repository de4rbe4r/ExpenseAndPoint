import { React, useState, useEffect } from 'react';
import axios from 'axios';
import Modal from 'react-bootstrap/Modal';
import Form from 'react-bootstrap/Form';
import { AddExpensesUrl, EditExpenseUrl } from "../../Urls/UrlList";
import Button from 'react-bootstrap/Button';
import AlertModal from '../AlertModal.jsx';
import Cookies from 'universal-cookie';

const ExpenseForm = ({ action, expenseToEdit, categoryList, isShowForm, setIsShowForm, userId, setIsDataUpdated, isDataUpdated }) => {
    const cookies = new Cookies();

    // Формирование текущей даты
    var today = new Date();
    var day = today.getDate();
    if (day < 10) {
        day = '0' + day;
    }
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
        date: '',
        constPart: 'T',
        time: ''
    })
    const config = {
        headers: { Authorization: `Bearer ${cookies.get('access_token')}` }
    };

    const [expense, setExpense] = useState({
        amount: '',
        userId: '',
        dateTime: '',
        categoryId: ''
    });

    const [title, setTitle] = useState();
    const [errorMessage, setErrorMessage] = useState("");
    const [showAlertModal, setShowAlertModal] = useState(false);
    const [titleAlert, setTitleAlert] = useState("");

    useEffect(() => {
        if (action === "Добавить") {
            setTitle("Добавление расхода");
        } else if (action === "Изменить") {
            setTitle("Изменение расхода");
        }

        setDateAndTime({
            ...dateAndTime,
            date: currentDate,
            constPart: 'T',
            time: hours + ":" + minutes
        })

        if (expenseToEdit !== undefined) {
            setExpense({
                ...expense,
                id: expenseToEdit.id,
                amount: expenseToEdit.amount,
                userId: expenseToEdit.userId,
                dateTime: expenseToEdit.dateTime,
                categoryId: expenseToEdit.categoryId
            });
            setDateAndTime({
                ...dateAndTime,
                date: expenseToEdit.dateTime.substr(0, 10),
                constPart: 'T',
                time: expenseToEdit.dateTime.substr(11, 8)
            })
        } else {
            setExpense({
                ...expense,
                id: 0,
                amount: '',
                userId: userId,
                dateTime: dateAndTime.date.toString() + dateAndTime.constPart + dateAndTime.time.toString(),
                categoryId: ((categoryList !== undefined) ? categoryList[0]?.id : 0)
            });
        }
    }, [isShowForm]);

    const changeDate = (event) => {
        setDateAndTime({ ...dateAndTime, date: event.target.value });
        setExpense({ ...expense, dateTime: event.target.value + dateAndTime.constPart + dateAndTime.time.toString() });
    }

    const changeTime = (event) => {
        setDateAndTime({ ...dateAndTime, time: event.target.value });
        setExpense({ ...expense, dateTime: dateAndTime.date.toString() + dateAndTime.constPart + event.target.value });
    }

    const handleCloseForm = () => {
        setExpense({
            ...expense,
            id: 0
        })
        setIsShowForm(false);
    }

    const addEditExpense = (event) => {
        event.preventDefault();
        if (expense.amount === "") {
            setShowAlertModal(true);
            setErrorMessage("Введите сумму");
            setTitleAlert("Ошибка");
            return;
        }

        if (action === "Добавить") {
            axios.post(AddExpensesUrl, expense, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Расход успешно добавлен");
                    setTitleAlert("Успешно");
                    setExpense({
                        ...expense,
                        amount: '',
                    });
                    setDateAndTime({
                        time: hours + ":" + minutes,
                        date: currentDate,
                        constPart: 'T'
                    })
                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");
                    }
                });
        } else if (action === "Изменить") {
            axios.put(EditExpenseUrl + expense.id, expense, config)
                .then(res => {
                    setShowAlertModal(true);
                    setErrorMessage("Расход успешно изменен");
                    setTitleAlert("Успешно");                })
                .catch(function (error) {
                    if (error.response) {
                        setShowAlertModal(true);
                        setErrorMessage(error.message + ". " + error.response.data.detail);
                        setTitleAlert("Ошибка");                    }
                });
            setIsShowForm(false);
        }
        setIsDataUpdated(!isDataUpdated);
    }

    const setIsShowAlertModal = (data) => {
        setShowAlertModal(data);
    }


    return (
        <>
            <AlertModal showModal={showAlertModal} setIsShowModal={setIsShowAlertModal} errorMessage={errorMessage} title={titleAlert} />
            <Modal
                show={isShowForm}
                onHide={handleCloseForm}
                backdrop="static"
                keyboard={false}
            >
                <Modal.Header closeButton>
                    <Modal.Title>{title}</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Label htmlFor="inputCategoryId">Категория</Form.Label>
                    <Form.Select
                        id="categoryId" onChange={event => {
                            setExpense({ ...expense, categoryId: event.target.value})
                        }}>
                        {(categoryList === undefined) ? null :
                            categoryList.map((c, index) => (
                            <option value={c.id} key={c.id}>{c.title}</option>
                        ))}
                    </Form.Select>
                    <Form.Label htmlFor="inputDate">Дата</Form.Label>
                    <Form.Control
                        type="date"
                        id="inputDate"
                        defaultValue={(expenseToEdit !== undefined) ? expenseToEdit.dateTime.substr(0, 10) : dateAndTime.date}
                        onChange={changeDate}
                    />
                    <Form.Label htmlFor="inputTime">Time</Form.Label>
                    <Form.Control
                        type="time"
                        id="inputTime"
                        defaultValue={(expenseToEdit !== undefined) ? expenseToEdit.dateTime.substr(11, 8) : dateAndTime.time}
                        onChange={changeTime}
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
                    <Button variant="primary" onClick={addEditExpense}>{action}</Button>
                </Modal.Footer>
            </Modal>
        </>
    )
}

export default ExpenseForm;