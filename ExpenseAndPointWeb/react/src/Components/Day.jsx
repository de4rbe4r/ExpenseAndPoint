import { React, useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import Row from 'react-bootstrap/Row';
import Col from 'react-bootstrap/Col';
import { GetExpensesListByUserIdAndDateUrl, GetCategoryByIdUrl } from "../Urls/UrlList";
import Cookies from 'universal-cookie';
import AddCategoryBtn from './AddCategoryBtn'
import ExpenseForm from './ExpenseForm.jsx'
import ExpensesList from './ExpensesList.jsx';
import PieChart from './PieChart.jsx';
import axios from 'axios';
import Button from 'react-bootstrap/Button';



const Day = () => {
    const cookies = new Cookies();

    const [expensesList, setExpensesList] = useState([{
        id: 0,
        value: {
            id: 0,
            amount: 0,
            userId: 0,
            dateTime: '1001-01-01T00:00:00',
            categoryId: 0
        }
    }]);
    const [categoryList, setCategoryList] = useState([]);
    const [isDataUpdated, setDataUpdated] = useState(false);

    var today = new Date();
    var date = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();

    const [userId, setUserId] = useState(cookies.get('userId'));

    useEffect(() => {
        axios.get(GetCategoryByIdUrl + userId)
            .then(res => {
                setCategoryList(res.data)
            })
            .catch(function (error) {
                if (error.response) {
                    if (error.request.status === 404) {
                        setExpensesList([{
                            id: 0,
                            value: {
                                id: 0,
                                amount: 0,
                                userId: 0,
                                dateTime: '1001-01-01T00:00:00',
                                categoryId: 0
                            }
                        }])
                    } else {
                        alert(error.message + '\n' + error.response.data.detail);
                    }
                }
            });
    }, [isDataUpdated]);

    useEffect(() => {
        axios.post(GetExpensesListByUserIdAndDateUrl, getRequest)
            .then(res => {
                setExpensesList(res.data);
            })
            .catch(function (error) {
                if (error.response) {
                    if (error.request.status !== 404) {
                        alert(error.message + '\n' + error.response.data.detail);
                    } 
                }
            });
    }, [isDataUpdated]);

    var getRequest = {
        date: date,
        userId: userId,
    }

    const [showForm, setShowForm] = useState(false);

    const setIsShowForm = (data) => {
        setShowForm(data);
    }

    const setIsDataUpdated = (data) => {
        setDataUpdated(data);
    }

    const openForm = (event) => {
        event.preventDefault();
        setShowForm(true);
    }

    return (
        <>
            <Container>
                <ExpenseForm action="Добавить" setIsDataUpdated={setIsDataUpdated} categoryList={categoryList} expenseToEdit={undefined}
                    isShowForm={showForm} setIsShowForm={setIsShowForm} userId={userId} isDataUpdated={isDataUpdated} />
                <Row>
                    <Col sm={4}>
                        <ExpensesList expensesList={expensesList} categoryList={categoryList} userId={userId} setIsDataUpdatedToParent={setIsDataUpdated}
                            isDataUpdated={isDataUpdated} />
                    </Col>
                    <Col sm={7}>
                        <Row>
                            <Col className="d-grid">
                                <Button variant="outline-light" size="lg" onClick={openForm}>Добавить расход</Button>
                            </Col>
                            <Col className="d-grid">
                                <AddCategoryBtn />
                            </Col>
                        </Row>
                        <Row>
                            <p></p>
                            <PieChart expensesList={expensesList} categoryList={categoryList} />
                        </Row>
                    </Col>
                </Row>
            </Container>
        </>
    );
}

export default Day;